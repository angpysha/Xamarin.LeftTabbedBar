using System;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Graphics;
using Color = Android.Graphics.Color;
using Math = Java.Lang.Math;
using View = Android.Views.View;

namespace Plugin.Angpysha.LeftTabbedPage.Android.Controls
{
    public class LeftTabbedFragment : Fragment
    {
        private Page _page;
        private WeakReference _weakReference;

        internal bool isInited = false;

        public LeftTabbedFragment(Page page, WeakReference weakRenderer) : base()
        {
            _page = page;
            _weakReference = weakRenderer;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
          //  var view = inflater.Inflate(Resource.Layout.left_tabbed_fragment, container, false);


            var dm = Context.Resources.DisplayMetrics;

            var screenWidthPixels = dm.WidthPixels;
            var tabWidthPixes = TypedValue.ApplyDimension(ComplexUnitType.Dip, 64, dm);
            var newWidthPixels = screenWidthPixels - tabWidthPixes;
            var newWidth = pxToDp((int)newWidthPixels, dm);
            var heightPixels = dm.HeightPixels;
            var height = pxToDp(heightPixels, dm);
            var formsPageRenderer = Platform.GetRenderer(_page);
            if (formsPageRenderer == null)
            {
                 Platform.SetRenderer(_page,Platform.CreateRendererWithContext(_page, Context));
                formsPageRenderer = Platform.GetRenderer(_page);
            }
           // Platform.SetRenderer(_page, formsPageRenderer);

            formsPageRenderer.Element.Layout(new Rectangle(0,0,newWidth, height));

            var layoutParams = new FrameLayout.LayoutParams((int)newWidthPixels, heightPixels);
            var nativeView = formsPageRenderer.View;

            var pageContainer = new LeftTabbedPageContainer(Context, formsPageRenderer, true);
            if (_weakReference.Target is LeftTabbedPageRenderer leftTabbedPageRenderer && leftTabbedPageRenderer.Element.Children.LastOrDefault() == _page)
            {
                leftTabbedPageRenderer.GoToStartPage();
            }
            return pageContainer;
        }

      

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            if (_weakReference.Target is LeftTabbedPageRenderer leftTabbedPageRenderer)
            {
                leftTabbedPageRenderer.GoToStartPage();
            }
        }

        public int pxToDp(int px, DisplayMetrics dm)
        {

            return (int)Math.Round(px / (dm.Xdpi / (double)global::Android.Util.DisplayMetricsDensity.Default));
        }

        public void UpdateLayout()
        {
            if (View is LeftTabbedPageContainer leftTabbedPageContainer)
            {
                isInited = true;
                var dm = Context.Resources.DisplayMetrics;

                var screenWidthPixels = dm.WidthPixels;
                var tabWidthPixes = TypedValue.ApplyDimension(ComplexUnitType.Dip, 64, dm);
                var newWidthPixels = screenWidthPixels - tabWidthPixes;
                var newWidth = pxToDp((int)newWidthPixels, dm);
                var heightPixels = dm.HeightPixels;
                var height = pxToDp(heightPixels, dm);
                var element = leftTabbedPageContainer.Child.Element;
                element.Layout(new Rectangle(0,0,newWidth,height));

                leftTabbedPageContainer.Child.UpdateLayout();

            }
        }
    }
}