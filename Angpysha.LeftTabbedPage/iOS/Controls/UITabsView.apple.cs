using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using Plugin.Angpysha.LeftTabbedPage.Shared;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


namespace Plugin.Angpysha.LeftTabbedPage
{
    public class UITabsView : UIView, IUITableViewDataSource, IUITableViewDelegate
    {
        public List<Shared.MenuItem> MenuItems { get; set; }
        private UIVerticalTabbar _tabsView;
        private UITableView table;

        public WeakReference<Shared.LeftTabbedPage> LeftTabbedPageWeak { get; set; }

        public Shared.LeftTabbedPage LeftTabbedPage
        {
            get
            {
                var result = LeftTabbedPageWeak.TryGetTarget(out var leftTabbedPage);
                return result ? leftTabbedPage : null;
            }
        }
        public UITabsView()
        {
            var height = UIScreen.MainScreen.Bounds.Height;
            table = new UITableView(new CGRect(0, 15, 64, height));
            table.ScrollEnabled = false;
            table.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            AddSubview(table);
            table.WeakDataSource = this;
            table.WeakDelegate = this;
            table.RegisterNibForCellReuse(UIVerticalTabbarViewCell.Nib, UIVerticalTabbarViewCell.Key);
        }

        public void SetData(List<Shared.MenuItem> menuItems,bool setPath = true)
        {
            MenuItems = menuItems;
            var rowHeight = UIScreen.MainScreen.Bounds.Height / MenuItems.Count;
            table.RowHeight = 64;
            if (setPath)
            {
                var path = NSIndexPath.FromRowSection(0, 0);
                SetTab(path);
            }
            // _tabsView.SetItems(MenuItems);
        }

        public void ReloadData()
        {
            table.ReloadData();
        }
        

        public nint RowsInSection(UITableView tableView, nint section)
        {
            return MenuItems?.Count ?? 0;
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (LeftTabbedPage.TabItemTemplate != null)
            {
                if (!(LeftTabbedPage.TabItemTemplate.CreateContent() is ViewCellEx cell))
                {
                    return null;
                }
                cell.BindingContext = MenuItems[indexPath.Row];
                cell.Parent = LeftTabbedPage;

                var rect = new Rectangle(0, 0, cell.View.WidthRequest, cell.View.HeightRequest);

                cell.View.Layout(rect);

                if (Platform.GetRenderer(cell.View) == null)
                {
                    Platform.SetRenderer(cell.View, Platform.CreateRenderer(cell.View));
                }

                var renderer = Platform.GetRenderer(cell.View);

                var nativeView = renderer.NativeView;
                nativeView.AutoresizingMask = UIViewAutoresizing.All;
                nativeView.ContentMode = UIViewContentMode.ScaleToFill;
                nativeView.Frame = new CGRect(0, 0, cell.View.WidthRequest, cell.View.HeightRequest);
                nativeView.SetNeedsLayout();


                var nativeCell = new UITableViewCell();
                nativeCell.Frame = new CGRect(0, 0, cell.View.WidthRequest, cell.View.HeightRequest);
                nativeCell.SelectionStyle = UITableViewCellSelectionStyle.None;
                foreach (var subView in nativeCell.ContentView.Subviews)
                {
                    subView.RemoveFromSuperview();
                }

                nativeCell.AddSubview(nativeView);
                return nativeCell;
            }
            else
            {
                var cell = tableView.DequeueReusableCell(UIVerticalTabbarViewCell.Key);
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                var item = MenuItems[indexPath.Row];
                if (cell is UIVerticalTabbarViewCell verticalTabbarViewCell)
                {
                    verticalTabbarViewCell.TitleView.Text = item.Title;
                   // verticalTabbarViewCell.IconView.Image = new UIImage(item.Icon).ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                   var imageSource = item.IconImageSource;
                   if (imageSource is FileImageSource fileImageSource)
                   {
                       var fileImageSOurceHandler = new FileImageSourceHandler();
                       var image = fileImageSOurceHandler.LoadImageAsync(imageSource).Result;
                       if (image != null)
                       {
                           verticalTabbarViewCell.IconView.Image =
                               image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                       }
                   }

                    if (indexPath.Row == 0)
                    {
                        verticalTabbarViewCell.TitleView.TextColor = UIColor.Orange;
                        verticalTabbarViewCell.IconView.TintColor = UIColor.Orange;
                    }
                    else
                    {
                        verticalTabbarViewCell.TitleView.TextColor = UIColor.Gray;
                        verticalTabbarViewCell.IconView.TintColor = UIColor.Gray;
                    }
                }

                return cell;
            }
        }

        public event EventHandler<int> TabSelected = delegate { };

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            TabSelected(this, indexPath.Row);
            
            SetTab(indexPath);
        }

        [Export("tableView:viewForHeaderInSection:")]
        public UIView ViewForHeaderInSection(UITableView tableView, nint section)
        {
            if (LeftTabbedPage.Header != null)
            {
                var view = LeftTabbedPage.Header;

                var rect = new Rectangle(0, 0, view.WidthRequest, view.HeightRequest);

                view.BindingContext = LeftTabbedPage.BindingContext;

                view.Parent = LeftTabbedPage;
                view.Layout(rect);

                if (Platform.GetRenderer(view) == null)
                {
                    Platform.SetRenderer(view, Platform.CreateRenderer(view));
                }

                var renderer = Platform.GetRenderer(view);
                var nativeView = renderer.NativeView;

                nativeView.AutoresizingMask = UIViewAutoresizing.All;
                nativeView.ContentMode = UIViewContentMode.ScaleToFill;
                nativeView.Frame = new CGRect(0, 0, view.WidthRequest, view.HeightRequest);
                nativeView.SetNeedsLayout();

                return nativeView;
            }
            return new UIView(new CGRect(0,0,0,0));
        }

        [Export("tableView:heightForHeaderInSection:")]
        public nfloat HeightForHeaderInSection(UITableView tableView, nint section)
        {
            return LeftTabbedPage.HeaderHeight;
        }
        private nfloat _footerHeight;

        [Export("tableView:heightForFooterInSection:")]
        public nfloat HeightForFooterInSection(UITableView tableView, nint section)
        {
            var tableHeight = table.Frame.Height;

            var itemsHeight = MenuItems.Count * 64f;

            var footerHeight = tableHeight - itemsHeight - LeftTabbedPage.HeaderHeight- LeftTabbedPage.Footer.Margin.Bottom - LeftTabbedPage.Footer.Margin.Top;
            _footerHeight = (nfloat)footerHeight;
            return (nfloat)footerHeight;
        }

        [Export("tableView:viewForFooterInSection:")]
        public UIView ViewForFooterInSection(UITableView tableView, nint section)
        {
            if (LeftTabbedPage.Footer != null)
            {
                var view = LeftTabbedPage.Footer;
                //  var size = view.Measure(64, 1000);
                var height = view.HeightRequest;
                if (height < tableView.SectionFooterHeight)
                    height = tableView.EstimatedSectionFooterHeight;
                var rect = new Rectangle(0, 0, view.WidthRequest, _footerHeight);

                view.BindingContext = LeftTabbedPage.BindingContext;

                view.Parent = LeftTabbedPage;
                view.Layout(rect);

                if (Platform.GetRenderer(view) == null)
                {
                    Platform.SetRenderer(view, Platform.CreateRenderer(view));
                }

                var renderer = Platform.GetRenderer(view);
                var nativeView = renderer.NativeView;

                nativeView.AutoresizingMask = UIViewAutoresizing.All;
                nativeView.ContentMode = UIViewContentMode.ScaleToFill;
                nativeView.Frame = new CGRect(0, 0, view.WidthRequest, view.HeightRequest);
                nativeView.SetNeedsLayout();

                return nativeView;
            }
            return new UIView(new CGRect(0, 0, 0, 0));
        }

        public void SetTab(NSIndexPath index)
        {
            // var activePath = NSIndexPath.FromRowSection(index, 0);
            if (LeftTabbedPage.TabItemTemplate == null)
            {
                var activeCell = table.CellAt(index) as UIVerticalTabbarViewCell;

                for (int i = 0; i < MenuItems?.Count; i++)
                {
                    var path = NSIndexPath.FromRowSection(i, 0);
                    var celll = table.CellAt(path) as UIVerticalTabbarViewCell;
                    celll.TitleView.TextColor = UIColor.Gray;
                    celll.IconView.TintColor = UIColor.Gray;
                }

                activeCell.TitleView.TextColor = UIColor.Orange;
                activeCell.IconView.TintColor = UIColor.Orange;
            } else {
                foreach (var item in MenuItems)
                {
                    item.Active = false;
                }

                MenuItems[index.Row].Active = true;
            }
        }
    }
}

