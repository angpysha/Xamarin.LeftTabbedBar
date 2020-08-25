using System;
using System.Collections.Generic;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Platform = Xamarin.Forms.Platform.iOS.Platform;
using Forms = Xamarin.Forms.Forms;
using System.Collections.Specialized;
using Xamarin.Forms.Internals;
using System.Linq;
using Foundation;
using CoreGraphics;
using Plugin.Angpysha.LeftTabbedPage.iOS;
using Plugin.Angpysha.LeftTabbedPage.Shared;
using Xamarin.Essentials;

[assembly: ExportRenderer(typeof(LeftTabbedPage),typeof(LeftTabbedPageRenderer))]
namespace Plugin.Angpysha.LeftTabbedPage.iOS
{
    public class LeftTabbedPageRenderer : UIViewController, IVisualElementRenderer, IEffectControlProvider
    {
        public Shared.LeftTabbedPage leftTabbedPage => Element as Shared.LeftTabbedPage;
        public static void Init()
        {
        }

        UIColor _defaultBarColor;
        bool _defaultBarColorSet;
        bool _loaded;
        Size _queuedSize;
        int lastSelectedIndex;

        Page Page => Element as Page;

        UIPageViewController pageViewController;

        protected UIViewController SelectedViewController;
        protected IList<UIViewController> ViewControllers;

        protected IPageController PageController
        {
            get { return Page; }
        }

        protected Shared.LeftTabbedPage Tabbed
        {
            get { return (Shared.LeftTabbedPage)Element; }
        }

        //   protected TabsView TabBar;
        protected UITabsView TabBar;
        private NSLayoutConstraint tabBarWidth;

        public LeftTabbedPageRenderer()
        {
            ViewControllers = new UIViewController[0];

            pageViewController = new UIPageViewController(
                UIPageViewControllerTransitionStyle.Scroll,
                UIPageViewControllerNavigationOrientation.Horizontal,
                UIPageViewControllerSpineLocation.None
            );

            TabBar = new UITabsView
            {
                TranslatesAutoresizingMaskIntoConstraints = false

            };


            //TabBar.AddConstraint(NSLayoutConstraint.Create(TabBar,
            //    NSLayoutAttribute.Left, NSLayoutRelation.Equal,
            //    1, 0));
            //TabBar.AddConstraint(NSLayoutConstraint.Create(
            //    TabBar,
            //    NSLayoutAttribute.Width,
            //    NSLayoutRelation.Equal,
            //    1, 64));
            //TabBar.AddConstraint(NSLayoutConstraint.Create(TabBar,
            //    NSLayoutAttribute.Height,
            //    NSLayoutRelation.GreaterThanOrEqual,
            //    1,
            //    ))
            //TabBar = new TabsView
            //{
            //    TranslatesAutoresizingMaskIntoConstraints = false
            //};
            //TabBar.TabsSelectionChanged += HandleTabsSelectionChanged;
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);

            View.SetNeedsLayout();
        }

        public override void ViewDidAppear(bool animated)
        {
            PageController.SendAppearing();
            base.ViewDidAppear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            PageController.SendDisappearing();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //table.Layer.MasksToBounds = false;
            //table.Layer.ShadowRadius = 10.0f;
            //table.Layer.ShadowColor = Color.Black.ToCGColor();
            //table.Layer.ShadowOffset = new CGSize(0.0f, 4.0f);
            //table.Layer.ShadowOpacity = 0.25f;
            //table.Layer.BorderColor = UIColor.LightGray.CGColor;
            //table.Layer.BorderWidth = 0.1f;
            TabBar.Layer.MasksToBounds = false;
            TabBar.Layer.ShadowRadius = 5f;
            TabBar.Layer.ShadowOffset = new CGSize(0.0f, 4.0f);
            TabBar.Layer.ShadowOpacity = 0.25f;
            TabBar.Layer.BorderColor = Color.LightGray.ToCGColor();
            TabBar.Layer.BorderWidth = 1f;

            TabBar.TabSelected += TabBar_TabSelected;
            View.AddSubview(TabBar);

            AddChildViewController(pageViewController);
            View.AddSubview(pageViewController.View);
            pageViewController.View.TranslatesAutoresizingMaskIntoConstraints = false;
            pageViewController.DidMoveToParentViewController(this);


            var views = NSDictionary.FromObjectsAndKeys(
                new NSObject[] {
                TabBar,
                pageViewController.View
            },
                new NSObject[] {
                (NSString) "tabbar",
                (NSString) "content"
            }
            );

            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-0-[tabbar]-0-|",
                                                                    0,
                                                                    null,
                                                                    views));
            //View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-0-[tabbar]-0-|",
            //                                                        0,
            //                                                        null,
            //                                                        views));
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-0-[content]-0-|",
                                                                    0,
                                                                    null,
                                                                    views));

            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-0-[tabbar]-0-[content]-0-|",
                                                                    0,
                                                                    null,
                                                                    views));

            tabBarWidth = NSLayoutConstraint.Create(
                TabBar,
                NSLayoutAttribute.Width,
                NSLayoutRelation.Equal,
                1, 64
            );
            TabBar.AddConstraint(tabBarWidth);
            //TabBar.BackgroundColor = UIColor.Red;

            if (pageViewController.ViewControllers.Length == 0
                && lastSelectedIndex >= 0 && lastSelectedIndex < ViewControllers.Count)
            {
                pageViewController.SetViewControllers(
                    new[] { ViewControllers[lastSelectedIndex] },
                   UIPageViewControllerNavigationDirection.Forward,
                   true, null
                );
            }

            // UpdateSwipe(Tabbed.SwipeEnabled);
            pageViewController.DidFinishAnimating += HandlePageViewControllerDidFinishAnimating;
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PageController?.SendDisappearing();

                if (Tabbed != null)
                {
                    Tabbed.PropertyChanged -= OnPropertyChanged;
                    Tabbed.PagesChanged -= OnPagesChanged;
                    //    TabBar.TabsSelectionChanged -= HandleTabsSelectionChanged;
                    TabBar.TabSelected -= TabBar_TabSelected;
                }

                if (pageViewController != null)
                {
                    pageViewController.WeakDataSource = null;
                    pageViewController.DidFinishAnimating -= HandlePageViewControllerDidFinishAnimating;
                    pageViewController?.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        protected virtual void OnElementChanged(VisualElementChangedEventArgs e)
        {
            ElementChanged?.Invoke(this, e);
            if (e.NewElement != null)
            {
                TabBar.LeftTabbedPageWeak = new WeakReference<Shared.LeftTabbedPage>(leftTabbedPage);
            }
        }

        UIViewController GetViewController(Page page)
        {
            
            var renderer = Platform.GetRenderer(page);
            return renderer?.ViewController;
        }

        void OnPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != Page.TitleProperty.PropertyName)
                return;

            if (!(sender is Page page) || page.Title is null)
                return;

            //   TabBar.ReplaceItem(page.Title, Tabbed.Children.IndexOf(page));
        }

        void OnPagesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            e.Apply((o, i, c) => SetupPage((Page)o, i), (o, i) => TeardownPage((Page)o, i), Reset);

            SetControllers();

            UIViewController controller = null;
            if (Tabbed.CurrentPage != null)
            {
                controller = GetViewController(Tabbed.CurrentPage);
            }

            if (controller != null && controller != SelectedViewController)
            {
                SelectedViewController = controller;
                var index = ViewControllers.IndexOf(SelectedViewController);
                MoveToByIndex(index);
                //  TabBar.SelectedIndex = index;
            }
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TabbedPage.CurrentPage))
            {
                var current = Tabbed.CurrentPage;
                if (current == null)
                    return;

                var controller = GetViewController(current);
                if (controller == null)
                    return;

                SelectedViewController = controller;
                var index = ViewControllers.IndexOf(SelectedViewController);
                MoveToByIndex(index);
                //  TabBar.SelectedIndex = index;
            }
            //else if (e.PropertyName == TabbedPage.BarBackgroundColorProperty.PropertyName)
            //{
            //    UpdateBarBackgroundColor();
            //}
            //else if (e.PropertyName == TabbedPage.BarTextColorProperty.PropertyName)
            //{
            //    UpdateBarTextColor();
            //}
            //else if (e.PropertyName == TopTabbedPage.BarIndicatorColorProperty.PropertyName)
            //{
            //    UpdateBarIndicatorColor();
            //}
            //else if (e.PropertyName == TopTabbedPage.SwipeEnabledColorProperty.PropertyName)
            //{
            //    UpdateSwipe(Tabbed.SwipeEnabled);
            //}
        }

        public override UIViewController ChildViewControllerForStatusBarHidden()
        {
            var current = Tabbed.CurrentPage;
            if (current == null)
                return null;

            return GetViewController(current);
        }

        void UpdateSwipe(bool swipeEnabled)
        {
            pageViewController.WeakDataSource = swipeEnabled ? this : null;
        }

        void Reset()
        {
            var i = 0;
            foreach (var page in Tabbed.Children)
            {
                SetupPage(page, i++);
            }
        }

        void SetControllers()
        {
            var list = new List<UIViewController>();
            var titles = new List<string>();
            for (var i = 0; i < Tabbed.Children.Count; i++)
            {
                var child = Tabbed.Children[i];
                //var width = DeviceDisplay.MainDisplayInfo.Width - 64;
                //child.WidthRequest = width;
                //var v = child as VisualElement;
                
                //if (v == null)
                //    continue;

                var renderer = Platform.GetRenderer(child);
                if (renderer == null) continue;

                //var frameOld = renderer.ViewController.View.Frame;
                //frameOld.Width = (nfloat)width;
                //renderer.ViewController.View.Frame = frameOld;
                list.Add(renderer.ViewController);

                titles.Add(Tabbed.Children[i].Title);
            }
            ViewControllers = list.ToArray();
            var menuitems = Tabbed.Children.Select(x => new Shared.MenuItem()
            {
                Title = x.Title,
                IconImageSource = x.IconImageSource
            }).ToList();
            TabBar.SetData(menuitems);
        }

        void SetupPage(Page page, int index)
        {
            IVisualElementRenderer renderer = Platform.GetRenderer(page);
            if (renderer == null)
            {
                renderer = Platform.CreateRenderer(page);
                Platform.SetRenderer(page, renderer);
            }

            page.PropertyChanged -= OnPagePropertyChanged;
            page.PropertyChanged += OnPagePropertyChanged;
        }

        void TeardownPage(Page page, int index)
        {
            page.PropertyChanged -= OnPagePropertyChanged;

            Platform.SetRenderer(page, null);
        }

        //void UpdateBarBackgroundColor()
        //{
        //    if (Tabbed == null || TabBar == null)
        //        return;

        //    var barBackgroundColor = Tabbed.BarBackgroundColor;

        //    if (!_defaultBarColorSet)
        //    {
        //        _defaultBarColor = TabBar.BackgroundColor;

        //        _defaultBarColorSet = true;
        //    }

        //    TabBar.BackgroundColor = barBackgroundColor.ToUIColor();
        //}

        //void UpdateBarTextColor()
        //{
        //    TabBar.TextColor = Tabbed.BarTextColor.ToUIColor();
        //}

        //void UpdateBarIndicatorColor()
        //{
        //    TabBar.IndicatorColor = Tabbed.BarIndicatorColor.ToUIColor();
        //}


        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

        public VisualElement Element { get; private set; }

        public UIView NativeView => View;

        public UIViewController ViewController => this;

        public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return NativeView.GetSizeRequest(widthConstraint, heightConstraint);
        }

        void IEffectControlProvider.RegisterEffect(Effect effect)
        {
            VisualElementRenderer<VisualElement>.RegisterEffect(effect, View);
        }

        public void SetElement(VisualElement element)
        {
            var oldElement = Element;
            Element = element;

            Tabbed.PropertyChanged += OnPropertyChanged;
            Tabbed.PagesChanged += OnPagesChanged;

            OnElementChanged(new VisualElementChangedEventArgs(oldElement, element));

            OnPagesChanged(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            if (element != null)
            {
                element.SendViewInitialized(NativeView);
            }

            //UpdateBarBackgroundColor();
            //UpdateBarTextColor();
            // UpdateBarIndicatorColor();

            EffectUtilities.RegisterEffectControlProvider(this, oldElement, element);
        }

        public void SetElementSize(Size size)
        {
            if (_loaded)
                Element.Layout(new Rectangle(Element.X, Element.Y, size.Width, size.Height));
            else
                _queuedSize = size;
        }


        public override void DidMoveToParentViewController(UIViewController parent)
        {
            base.DidMoveToParentViewController(parent);

            var parentFrame = ParentViewController.View.Frame;

            tabBarWidth.Constant = 64;

            //SetElementSize(new Size(parentFrame.Width, parentFrame.Height));
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            if (Element == null)
                return;
            if (!Element.Bounds.IsEmpty)
            {
                View.Frame = new System.Drawing.RectangleF((float)Element.X, (float)Element.Y, (float)Element.Width, (float)Element.Height);
            }

            //  var tabsFrame = TabBar.Frame;
            var frame = ParentViewController != null ? ParentViewController.View.Frame : View.Frame;
            //var height = frame.Height - tabsFrame.Height;
            //PageController.ContainerArea = new Rectangle(0, 0, frame.Width, height);

            if (!_queuedSize.IsZero)
            {
                Element.Layout(new Rectangle(Element.X, Element.Y, _queuedSize.Width, _queuedSize.Height));
                _queuedSize = Size.Zero;
            }

            pageViewController.SetViewControllers(pageViewController.ViewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);

            _loaded = true;
        }

        public UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var index = ViewControllers.IndexOf(referenceViewController) - 1;
            if (index < 0) return null;

            return ViewControllers[index];
        }

        public UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var index = ViewControllers.IndexOf(referenceViewController) + 1;
            if (index == ViewControllers.Count) return null;

            return ViewControllers[index];
        }

        void HandlePageViewControllerDidFinishAnimating(object sender, UIPageViewFinishedAnimationEventArgs e)
        {
            if (pageViewController.ViewControllers.Length == 0) return;

            SelectedViewController = pageViewController.ViewControllers[0];
            var index = ViewControllers.IndexOf(SelectedViewController);

            //     TabBar.SelectedIndex = index;
            lastSelectedIndex = index;
            UpdateToolbarItems(index);
        }

        private void TabBar_TabSelected(object sender, int e)
        {
            MoveToByIndex(e);
        }

        public virtual void MoveToByIndex(int selectedIndex, bool forced = false)
        {
            if (selectedIndex == lastSelectedIndex && !forced) return;

            var nextPage = Tabbed.Children.ElementAt(selectedIndex);
            if (Tabbed.CurrentPage != nextPage)
            {
                Tabbed.CurrentPage = nextPage;
                return;
            }

            var direction = lastSelectedIndex < selectedIndex
                             ? UIPageViewControllerNavigationDirection.Forward
                             : UIPageViewControllerNavigationDirection.Reverse;

            lastSelectedIndex = selectedIndex;

            SelectedViewController = ViewControllers[lastSelectedIndex];

            UpdateToolbarItems(selectedIndex);

            pageViewController.SetViewControllers(
                new[] { SelectedViewController },
                direction,
                false, (finished) =>
                {
                    var failed = pageViewController.ViewControllers.Length == 0
                                                   || pageViewController.ViewControllers[0] != SelectedViewController;
                    if (failed)
                    {
                        //Sometimes setViewControllers doesn't work as expected
                        pageViewController.SetViewControllers(
                            new[] { SelectedViewController },
                            direction,
                            false, null);
                    }
                }
            );
        }

        void UpdateToolbarItems(int selectedIndex)
        {
            if (NavigationController == null) return;

            var toolbarItems = new List<ToolbarItem>(Tabbed.ToolbarItems);

            var newChild = Tabbed.Children[selectedIndex];
            var navigationItem = this.NavigationController.TopViewController.NavigationItem;
            toolbarItems.AddRange(newChild.ToolbarItems);

            navigationItem.SetRightBarButtonItems(toolbarItems.Select(x => x.ToUIBarButtonItem()).ToArray(), false);
        }
    }

    static class ElementExtensions
    {
        public static void SendViewInitialized(this VisualElement element, UIView nativeView)
        {
            var method = typeof(Xamarin.Forms.Forms).GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                                                    .FirstOrDefault(x => x.Name == nameof(SendViewInitialized));

            method.Invoke(null, new object[] { element, nativeView });
        }
    }

}
