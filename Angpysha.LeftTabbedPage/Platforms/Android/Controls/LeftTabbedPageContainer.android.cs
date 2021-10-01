using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Graphics;

namespace Plugin.Angpysha.LeftTabbedPage.Android.Controls
{
    public class LeftTabbedPageContainer : FrameLayout
    {
        public LeftTabbedPageContainer(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public LeftTabbedPageContainer(Context? context) : base(context)
        {
        }

        public LeftTabbedPageContainer(Context? context, IAttributeSet? attrs) : base(context, attrs)
        {
        }

        public LeftTabbedPageContainer(Context? context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public LeftTabbedPageContainer(Context? context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }


        public LeftTabbedPageContainer(Context context, IVisualElementRenderer renderer, bool inFragment = false) :
            this(context)
        {
            Id = global::Android.Views.View.GenerateViewId();
            Child = renderer;
            IsInFragment = inFragment;
            var nativeView = renderer.View;
            AddView(nativeView);
        }

        public IVisualElementRenderer Child { get; set; }

        public bool IsInFragment { get; set; }


        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            var dm = Context.Resources.DisplayMetrics;

            var screenWidthPixels = dm.WidthPixels;
            var tabWidthPixes = TypedValue.ApplyDimension(ComplexUnitType.Dip, 64, dm);
            var newWidthPixels = screenWidthPixels - tabWidthPixes;
            var newWidth = pxToDp((int)newWidthPixels, dm);
            var heightPixels = dm.HeightPixels;
            var height = pxToDp(heightPixels, dm);
            var element = Child.Element;
            element.Layout(new Rectangle(0, 0, newWidth, height));
            Child.UpdateLayout();
        }

        public int pxToDp(int px, DisplayMetrics dm)
        {

            return (int)Math.Round(px / (dm.Xdpi / (double)global::Android.Util.DisplayMetricsDensity.Default));
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            Child.View.Measure(widthMeasureSpec, heightMeasureSpec);
            SetMeasuredDimension(Child.View.MeasuredWidth, Child.View.MeasuredHeight);
        }

    }
}
