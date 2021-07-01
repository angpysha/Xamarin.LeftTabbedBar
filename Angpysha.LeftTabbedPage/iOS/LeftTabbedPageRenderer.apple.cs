using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;
using Color = Xamarin.Forms.Color;
using Platform = Xamarin.Forms.Platform.iOS.Platform;
using Rectangle = Xamarin.Forms.Rectangle;
using Size = Xamarin.Forms.Size;

namespace Plugin.Angpysha.LeftTabbedPage.iOS
{
    public class LeftTabbedPageRenderer : UIViewController, IVisualElementRenderer, IEffectControlProvider
    {
        #region vars

        private bool _loaded;
        private Size _queuedSize;
        private UITabsView _tabBarView;
        
        //its hardcoded, TODO: should be replaced
        private float _tabBarWidth = 64;
        
        private UIPageViewController _pageViewController;
        
        public VisualElement Element { get; private set; }
        public UIView NativeView => View;
        public UIViewController ViewController => this;
        public int LastSelectedIndex { get; set; }
        public Shared.LeftTabbedPage TabbedPage => (Shared.LeftTabbedPage) Element;

        #endregion

        public LeftTabbedPageRenderer()
        {
            _tabBarView = new UITabsView {TranslatesAutoresizingMaskIntoConstraints = false};
            _pageViewController = new UIPageViewController(
                UIPageViewControllerTransitionStyle.Scroll,
                UIPageViewControllerNavigationOrientation.Horizontal,
                UIPageViewControllerSpineLocation.None);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
             _tabBarView.Layer.MasksToBounds = false;
             _tabBarView.Layer.ShadowRadius = 5f;
             _tabBarView.Layer.ShadowOffset = new CGSize(0.0f, 4.0f);
             _tabBarView.Layer.ShadowOpacity = 0.25f;
             _tabBarView.Layer.BorderColor = Color.LightGray.ToCGColor();
             _tabBarView.Layer.BorderWidth = 1f;
             _tabBarView.TabSelected += OnTabSelected;
             NativeView.AddSubview(_tabBarView);
             AddChildViewController(_pageViewController);
             NativeView.AddSubview(_pageViewController.View);
             _pageViewController.View.TranslatesAutoresizingMaskIntoConstraints = false;
            
             var views = NSDictionary.FromObjectsAndKeys(
                 new NSObject[] {_tabBarView, _pageViewController.View},
                 new NSObject[] {(NSString) "tabbar", (NSString) "content"});
            
             NativeView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-0-[tabbar]-0-|",
                 0, null, views));
             NativeView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-0-[content]-0-|",
                 0, null, views));
             NativeView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-0-[tabbar]-0-[content]-0-|",
                 0, null, views));
            
             var tabBarWidthConstraint = NSLayoutConstraint.Create(_tabBarView, NSLayoutAttribute.Width,
                 NSLayoutRelation.Equal, 1, _tabBarWidth);
             _tabBarView.AddConstraint(tabBarWidthConstraint);
             

            // COMMENTED CODE: looks like useless
            // _pageViewController.DidFinishAnimating += OnDidFinishAnimating;
        }

        public override void ViewDidAppear(bool animated)
        {
            ((IPageController)Element).SendAppearing();
            base.ViewDidAppear(animated);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            if (Element == null)
                return;

            var newWidth = (float) Element.Width + _tabBarWidth;
            if (!Element.Bounds.IsEmpty)
                NativeView.Frame = new RectangleF(0, (float) Element.Y, (float) Element.Width+_tabBarWidth,
                    (float) Element.Height);
            if (_pageViewController.ViewControllers.Length == 0
                && LastSelectedIndex >= 0 && LastSelectedIndex < TabbedPage.Children.Count)
            {
                _pageViewController.SetViewControllers(new[] {GetViewController(TabbedPage.CurrentPage)},
                    UIPageViewControllerNavigationDirection.Forward, true, null);
                
                //we use Children.IndexOf instead of CurrentPage.TabIndex because TabIndex is 0 here
                //TODO: why?
                LastSelectedIndex = TabbedPage.Children.IndexOf(TabbedPage.CurrentPage);
                SetTab(LastSelectedIndex);
            }
          //  OnTabSelected(this,0);
            //
            //
            // var pageVcFrame = _pageViewController.View.Frame;
            // // int iii = 0;
            // // var rect = _pageViewController.View.Bounds.ToRectangle();
            // // rect.X = 0;
            // // //rect.Width += _tabBarWidth;
            // var rect = pageVcFrame.ToRectangle();
            // rect.X = 0;
            // int iii = 0;
            // TabbedPage.ContainerArea = rect;

            // if (!_queuedSize.IsZero)
            // {
            //     Element.Layout(new Rectangle(Element.X, Element.Y, _queuedSize.Width, _queuedSize.Height));
            //     _queuedSize = Size.Zero;
            // }
            // _loaded = true;
        }

        
        public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return NativeView.GetSizeRequest(widthConstraint, heightConstraint);
        }

        public void SetElement(VisualElement element)
        {
            var oldElement = Element;
            Element = element;

            TabbedPage.PropertyChanged += OnPropertyChanged;
            OnElementChanged(new VisualElementChangedEventArgs(oldElement, element));
            var menuItems = TabbedPage.Children
                .Select(x => new Shared.MenuItem {Title = x.Title, IconImageSource = x.IconImageSource}).ToList();
            _tabBarView.SetData(menuItems);
            
            element?.SendViewInitialized(NativeView);
            EffectUtilities.RegisterEffectControlProvider(this, oldElement, element);
        }

        public void ReloadTabs(List<Shared.MenuItem> menuItems)
        {
            _tabBarView.SetData(menuItems,false);
            _tabBarView.ReloadData();
        }
        public void SetElementSize(Size size)
        {
            var widthNew = size.Width - _tabBarWidth;
            // if (_loaded)
            Element.Layout(new Rectangle(_tabBarWidth, Element.Y, widthNew, size.Height));
            // else
            //     _queuedSize = size;
        }
        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;
        
        public void RegisterEffect(Effect effect)
        {
            VisualElementRenderer<VisualElement>.RegisterEffect(effect, View);
        }

        public override UIViewController ChildViewControllerForStatusBarHidden()
        {
            var current = TabbedPage.CurrentPage;
            if (current == null)
                return null;

            return GetViewController(current);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ((IPageController)Element).SendDisappearing();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                ((IPageController)Element)?.SendDisappearing();
                if (TabbedPage != null)
                {
                    TabbedPage.PropertyChanged -= OnPropertyChanged;
                    foreach (var page in TabbedPage.Children)
                    {
                        TeardownPageRenderer(page);
                    }
                }
                if(_tabBarView != null)
                    _tabBarView.TabSelected -= OnTabSelected;

                if (_pageViewController != null)
                {
                    _pageViewController.WeakDataSource = null;
                    //COMMENTED CODE: looks like useless
                    //_pageViewController.DidFinishAnimating -= OnDidFinishAnimating;
                    _pageViewController.Dispose();
                }
            }
        }

        #region additional methods

        private UIViewController GetViewController(Page page)
        {
            // var screenWidth = DeviceDisplay.MainDisplayInfo.Width;
            // var screenHeight = DeviceDisplay.MainDisplayInfo.Height;
            // page.WidthRequest = screenWidth - _tabBarWidth;
            // page.Layout(new Rectangle(0,0,screenWidth-_tabBarWidth,screenHeight));
        
            var renderer = page.GetRenderer();
        
            if (renderer == null)
            {
                SetupPageRenderer(page);
                renderer = page.GetRenderer();
            }
            return renderer.ViewController;
        }

        private void SetupPageRenderer(Page page)
        {
            var renderer = page.GetRenderer();
            if (renderer == null)
            {
                renderer = Platform.CreateRenderer(page);
                Platform.SetRenderer(page, renderer);
            }
        }

        private void TeardownPageRenderer(Page page)
        {
            var renderer = page?.GetRenderer();
            if (renderer != null)
                Platform.SetRenderer(page, null);
        }

        protected virtual void OnTabSelected(object sender, int index)
        {
            var selectedPage = TabbedPage.Children[index];
            SetupPageRenderer(selectedPage);
            var nextPage = TabbedPage.Children[index];
            if (TabbedPage.CurrentPage != nextPage)
            {
                TabbedPage.CurrentPage = nextPage;
            }
          //  MoveToByIndex(index);
        }

        public virtual void MoveToByIndex(int selectedIndex, bool forced = false)
        {
            if (selectedIndex == LastSelectedIndex && !forced)
                return;

            // var nextPage = TabbedPage.Children[selectedIndex];
            // if (TabbedPage.CurrentPage != nextPage)
            // {
            //     TabbedPage.CurrentPage = nextPage;
            // }

            var direction = LastSelectedIndex < selectedIndex
                ? UIPageViewControllerNavigationDirection.Forward
                : UIPageViewControllerNavigationDirection.Reverse;

            LastSelectedIndex = selectedIndex;
            var selectedViewController = GetViewController(TabbedPage.Children[LastSelectedIndex]);
            _pageViewController.SetViewControllers(new[] {selectedViewController},
                direction, false, (finished) =>
                {
                    var failed = _pageViewController.ViewControllers.Length == 0
                                 || _pageViewController.ViewControllers[0] != selectedViewController;
                    //sometimes SetViewControllers doesn't work as expected
                    //TODO: why?
                    if (failed)
                    {
                        _pageViewController.SetViewControllers(new[] {selectedViewController},
                            direction, false, null);
                    }
                });
        }

        public void SetTab(int index)
        {
            _tabBarView.SetTab(NSIndexPath.FromRowSection(index, 0));
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(Xamarin.Forms.TabbedPage.CurrentPage))
            {
                var current = TabbedPage.CurrentPage;
                if (current == null)
                    return;
                var controller = GetViewController(current);
                if (controller == null)
                    return;
                var index = TabbedPage.Children.IndexOf(TabbedPage.CurrentPage);
                MoveToByIndex(index);
            }
        }

        protected virtual void OnElementChanged(VisualElementChangedEventArgs e)
        {
            ElementChanged?.Invoke(this, e);
            if (e.NewElement != null)
            {
                _tabBarView.LeftTabbedPageWeak = new WeakReference<Shared.LeftTabbedPage>(TabbedPage);
            }
        }
        #endregion
    }
    
    static class ElementExtensions
    {
        public static void SendViewInitialized(this VisualElement element, UIView nativeView)
        {
            var method = typeof(Forms).GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .FirstOrDefault(x => x.Name == nameof(SendViewInitialized));

            method?.Invoke(null, new object[] { element, nativeView });
        }
    }
}
