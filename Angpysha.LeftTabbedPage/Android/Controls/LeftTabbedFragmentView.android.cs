using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

namespace Plugin.Angpysha.LeftTabbedPage.Android.Controls
{
    public class LeftTabbedFragment : Fragment
    {
        private Page _page;
        
        public LeftTabbedFragment(Page page) : base()
        {
            _page = page;
        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.left_tabbed_fragment, container, false);
         

            var dm = Context.Resources.DisplayMetrics;
            
            var screenWidthPixels = dm.WidthPixels;
            var tabWidthPixes = TypedValue.ApplyDimension(ComplexUnitType.Dip, 64, dm);
            var newWidthPixels = screenWidthPixels - tabWidthPixes;
            var newWidth = pxToDp((int)newWidthPixels, dm);
            var heightPixels = dm.HeightPixels;
            var height = pxToDp(heightPixels,dm);
            
            _page.Layout(new Rectangle(0,0, newWidth, height));

            var formsPageRenderer = Platform.GetRenderer(_page);
            if (formsPageRenderer == null)
            {
                Platform.SetRenderer(_page, Platform.CreateRendererWithContext(_page,Context));
                formsPageRenderer = Platform.GetRenderer(_page);
            }

            var frameView = view.FindViewById<FrameLayout>(Resource.Id.lefttabbed_page_container);
            var layoutParams = new FrameLayout.LayoutParams(newWidth, heightPixels);
            var nativeView = formsPageRenderer.View;
            
            nativeView.LayoutParameters = layoutParams;
            var paremt = nativeView.Parent;
            (paremt as ViewGroup)?.RemoveAllViews();
            frameView?.AddView(nativeView);
            
            
            return view;
        }
        
        public int pxToDp(int px, DisplayMetrics dm) {
            
            return (int)Math.Round(px / (dm.Xdpi / (double)global::Android.Util.DisplayMetricsDensity.Default));
        }
    }
}