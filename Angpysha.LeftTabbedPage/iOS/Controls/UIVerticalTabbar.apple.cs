using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UIKit;
using Xamarin.Forms;

namespace Plugin.Angpysha.LeftTabbedPage
{
    [DesignTimeVisible(true)]
    public partial class UIVerticalTabbar : UIView, IComponent, IUITableViewDataSource, IUITableViewDelegate
    {

        public List<Shared.MenuItem> MenuItems { get; set; }

        public UIVerticalTabbar (IntPtr handle) : base (handle)
        {
            SetupXib();
        }

        public UIVerticalTabbar(CGRect frame) : base(frame)
        {
            SetupXib();
        }

        public UIVerticalTabbar(NSCoder coder) : base(coder)
        {
            SetupXib();
        }

        public UIVerticalTabbar()
        {
            SetupXib();
        }

        public ISite Site { get; set; }

        public event EventHandler Disposed;

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            if ((Site != null) && Site.DesignMode)
            {
                // Bundle resources aren't available in DesignMode
                return;
            }

            NSBundle.MainBundle.LoadNib("UIVerticalTabbar", this, null);
          //  AddSubview(RootView);

          //  RootView.BackgroundColor = UIColor.Brown;
            //LayoutSubviews();
        }

        private void SetupXib()
        {
            var view = LoadNib();

            //   var height = UIScreen.MainScreen.Bounds.Height;
            //var frame = new CGRect(0, 0, 64, OutletTabs.Frame.Height);
            //view.Frame = frame;
            view.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            //   view.BackgroundColor = UIColor.Blue;
            AddSubview(view);
            //var table = new UITableView();
            //table.Frame = new CGRect(0, 0, 64, 800);
            //AddSubview(table);
            OutletTabs.WeakDataSource = this;
            OutletTabs.WeakDelegate = this;
            OutletTabs.RegisterNibForCellReuse(UIVerticalTabbarViewCell.Nib, UIVerticalTabbarViewCell.Key);
            OutletTabs.AllowsSelection = true;
            //var realHeight = 64 * MenuItems?.Count ?? 0;
            //if (realHeight != 0)
            //{
            //    if (realHeight < OutletTabs.Frame.Height)
            //    {
            //        var yOffset = (OutletTabs.Frame.Height - realHeight) / 2;
            //        OutletTabs.ContentOffset = new CGPoint(0, yOffset);
            //    }
            //}
            // OutletTabs.BackgroundColor = UIColor.Green;

        }

        private UIView LoadNib()
        {
            var nib = UINib.FromName("UIVerticalTabbar", NSBundle.MainBundle);

            return nib.Instantiate(this, null)[0] as UIView;
        }

        public nint RowsInSection(UITableView tableView, nint section)
        {
            return MenuItems?.Count()??0;
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(UIVerticalTabbarViewCell.Key);
            var item = MenuItems[indexPath.Row];
            if (cell is UIVerticalTabbarViewCell verticalTabbarViewCell)
            {
                if (!string.IsNullOrWhiteSpace(item.Icon))
                    verticalTabbarViewCell.IconView.Image = new UIImage(item.Icon);
                verticalTabbarViewCell.TitleView.Text = item.Title;
            }

            return cell;
        }

        public void SetItems(List<Shared.MenuItem> menuItems)
        {
            MenuItems = menuItems;
            OutletTabs?.ReloadData();
            
        }

        internal void ReloadData()
        {
            OutletTabs?.ReloadData();
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView,NSIndexPath indexPath)
        {
            int iii = 0;
        }
    }
}