using Android.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;
using Plugin.Angpysha.LeftTabbedPage;
using Android.Views;
using Plugin.Angpysha.LeftTabbedPage.Shared;
using Plugin.Angpysha.LeftTabbedPage.Android;
using AndroidX.RecyclerView.Widget;
using System.Linq;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Util;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.ViewPager2.Widget;
using Plugin.Angpysha.LeftTabbedPage.Android.Controls;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Graphics;
using Android.Graphics.Drawables;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.CardView.Widget;
using AndroidX.AppCompat.App;
using Microsoft.Maui.Essentials;
//using Xamarin.Essentials;
//using Platform = Xamarin.Forms.Platform.Android.Platform;

//[assembly: ExportRenderer(typeof(LeftTabbedPage), typeof(LeftTabbedPageRenderer))]
//Xamarin.Forms.PlatformConfiguration.Android
namespace Plugin.Angpysha.LeftTabbedPage.Android
{
    public class LeftTabbedPageRenderer : VisualElementRenderer<Shared.LeftTabbedPage>,
        IPlatformElementConfiguration<Microsoft.Maui.Controls.PlatformConfiguration.Android, Shared.LeftTabbedPage>
    {
        private bool inited = false;
        internal global::Android.Views.View view;
        private ViewPager2 _viewPager;


        internal RecyclerView _recyclerView;
        internal RecyclerView _recyclerViewPager;
        private RecyclerViewAdapter _tabAdapter;
        private FrameLayout _headerViewContainer;
        private FrameLayout _footerViewContainer;
        private ConstraintLayout _layout;
        private CardView _cardView;
        private RelativeLayout _cardContainer;


        public int CurrentIndex { get; set; }
        public int PreviousIndex { get; set; } = -1;

        public LeftTabbedPageRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(TabbedPage.CurrentPage))
            {
                // AdapterOnOnItemClicked();
                var index = Element.Children.IndexOf(Element.CurrentPage);
                _tabAdapter.OnClicked(index);
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Shared.LeftTabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
              //  this.Background = new ColorDrawable(Colors.Green.ToAndroid());
                var inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                var vg = this as ViewGroup;
                view = inflater.Inflate(Resource.Layout.activity_main_android, null, false);
          //      view.Background = new ColorDrawable(Colors.Red.ToAndroid());
                view.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
                AddView(view);

                // lefttabbed_recycler
                _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.lefttabbed_recycler);
             //   _recyclerView.Background = new ColorDrawable(Colors.Red.ToAndroid());
                _viewPager = view.FindViewById<ViewPager2>(Resource.Id.lefttabbed_viewpager);
                _headerViewContainer = view.FindViewById<FrameLayout>(Resource.Id.lefttabed_tabbar_header);
               // _headerViewContainer.SetBackgroundColor(Colors.Yellow.ToAndroid());
                _footerViewContainer = view.FindViewById<FrameLayout>(Resource.Id.lefttabed_tabbar_footer);
                _layout = view.FindViewById<ConstraintLayout>(Resource.Id.coordinatorLayout);
                _cardView = view.FindViewById<CardView>(Resource.Id.cardView);
                _cardContainer = _cardView.GetChildAt(0) as RelativeLayout;
               // _cardContainer = view.FindViewById<RelativeLayout>(Resource.Id.card_container);
             //   _cardContainer.SetBackgroundColor(Colors.PowderBlue.ToAndroid());
           //     _cardView.SetBackgroundColor(Colors.Yellow.ToAndroid());
             //   _layout.SetBackgroundColor(Colors.Purple.ToAndroid());

                SetMenuItems();
            }
        }



        internal LeftTabbedViewPagerAdapter _leftTabbedViewPagerAdapter;
        internal List<LeftTabbedFragment> Fragments { get; set; }

        private async void SetMenuItems()
        {
            var menuitems = Element.Children.Select(x => new Shared.MenuItem()
            {
                Title = x.Title,
                IconImageSource = x.IconImageSource
            }).ToList();
            menuitems[0].Active = true;
            _tabAdapter = new RecyclerViewAdapter(menuitems,
                new WeakReference<LeftTabbedPage.Shared.LeftTabbedPage>(Element), Context);
      //      _recyclerView.Background = new ColorDrawable(Colors.Purple.ToAndroid());
            _recyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _recyclerView.SetAdapter(_tabAdapter);
            _tabAdapter.OnItemClicked += AdapterOnOnItemClicked;
            if (Context.GetActivity() is FragmentActivity fragmentActivity)
            {
                Fragments = Element.Children.Select(x => new LeftTabbedFragment(x, new WeakReference(this))).ToList();
                var fomsActivity = (Context as AppCompatActivity);
                var lifecycle = fomsActivity.Lifecycle;
                var fragmentManager = Context.GetFragmentManager();
                  _leftTabbedViewPagerAdapter = new LeftTabbedViewPagerAdapter(fragmentManager, lifecycle, new WeakReference(this));
                //       var addaoter = new LeftTabbedRecyclerPagerAdapter(Element.Children.ToList(), new WeakReference<Shared.LeftTabbedPage>(Element), Context);
                _leftTabbedViewPagerAdapter.AddFragments(Fragments);
                _viewPager.Orientation = (int)Orientation.Vertical;
                MainThread.BeginInvokeOnMainThread(() => _viewPager.Adapter = _leftTabbedViewPagerAdapter);
             //   _viewPager.Adapter = addaoter;
              //  _viewPager.UserInputEnabled = false;
                _viewPager.RegisterOnPageChangeCallback(new PageChangedCallback(new WeakReference(this)));
              //  _viewPager.SetPageTransformer(null);

                view.ViewTreeObserver.AddOnGlobalLayoutListener(new GlobalLayoutListenr(new WeakReference(this)));

            }

            if (Element.Header != null)
            {
                var headerMeasure = Element.Header.Measure(double.MaxValue, double.MaxValue);
                var width = 64;

                var height = headerMeasure.Request.Height != 0
                    ? headerMeasure.Request.Height
                    : headerMeasure.Minimum.Height;

                if (Element.HeaderHeight != 0)
                {
                    height = Element.HeaderHeight;
                }
               // var tt = null;
             //   (tt as Microsoft.Maui.Controls.View).bac
                //  Element.Header.Layout(new Rectangle(0,0,width, height));
                var dm = Context.Resources.DisplayMetrics;
                var renderer = Microsoft.Maui.Controls.Compatibility.Platform.Android.Platform.CreateRendererWithContext(Element.Header, Context);
                renderer.Element.Layout(new Rectangle(0, 0, width, height));
                var widthPx = TypedValue.ApplyDimension(ComplexUnitType.Dip, width, dm);
                var heightPx = TypedValue.ApplyDimension(ComplexUnitType.Dip, (float)height, dm);
                var lp = new LayoutParams((int)widthPx, (int)heightPx);
                renderer.View.LayoutParameters = lp;
                //  renderer.Tracker.UpdateLayout();

                _headerViewContainer.AddView(renderer.View);
              //  _headerViewContainer.Layout(0, 0, (int)widthPx, (int)heightPx);
            }

            if (Element.Footer != null)
            {
                var fotterMeasure = Element.Footer.Measure(double.MaxValue, double.MaxValue);
                var width = 64;

                var height = fotterMeasure.Request.Height != 0
                    ? fotterMeasure.Request.Height
                    : fotterMeasure.Minimum.Height;

                //if (Element.fo != 0)
                //{
                //    height = Element.HeaderHeight;
                //}

                var dm = Context.Resources.DisplayMetrics;
                var renderer = Microsoft.Maui.Controls.Compatibility.Platform.Android.Platform.CreateRendererWithContext(Element.Footer, Context);
                renderer.Element.Layout(new Rectangle(0, 0, width, height));
                var widthPx = TypedValue.ApplyDimension(ComplexUnitType.Dip, width, dm);
                var heightPx = TypedValue.ApplyDimension(ComplexUnitType.Dip, (float)height, dm);
                var lp = new LayoutParams((int)widthPx, (int)heightPx);
                renderer.View.LayoutParameters = lp;

                _footerViewContainer.AddView(renderer.View);
            }
        }

        private void AdapterOnOnItemClicked(object sender, int e)
        {
            _viewPager.CurrentItem = e;

        }




        public class PageChangedCallback : ViewPager2.OnPageChangeCallback
        {
            private WeakReference _weakRenderer;

            private LeftTabbedPageRenderer renderer => _weakRenderer.Target as LeftTabbedPageRenderer;
            public PageChangedCallback(WeakReference renderer)
            {
                _weakRenderer = renderer;
            }

            public override void OnPageSelected(int position)
            {
                base.OnPageSelected(position);
                UpdateLayout();
            }

            public override void OnPageScrollStateChanged(int state)
            {
                base.OnPageScrollStateChanged(state);
                UpdateLayout();
            }

            private void UpdateLayout()
            {
                var pager = renderer._viewPager;
                var index = pager.CurrentItem;
                var currentFragment = renderer.Fragments[index];
                currentFragment.UpdateLayout();
            }
        }

        public class GlobalLayoutListenr : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
        {
            private WeakReference _weakRenderer;

            private LeftTabbedPageRenderer renderer => _weakRenderer.Target as LeftTabbedPageRenderer;
            public GlobalLayoutListenr(WeakReference renderer)
            {
                _weakRenderer = renderer;
            }

            private bool _skip = true;
            public async void OnGlobalLayout()
            {
                if (renderer.view.Height > 0 && renderer._recyclerView.Width > 0)
                {
                    //if (_skip)
                    //{
                    //    _skip = false;
                    //    return;
                    //}
                    var pager = renderer._viewPager;
                    var index = pager.CurrentItem;
                    renderer._leftTabbedViewPagerAdapter.NotifyItemChanged(index);
                    var currentFragment = renderer.Fragments[index];
                    currentFragment.UpdateLayout();

                    renderer.view.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);

                }
            }
        }

        internal void GoToStartPage()
        {
        }

        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            //if (inited)
            //    return;

            //inited = true;
            ////base.OnLayout(changed, l, t, r, b);
            //var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            //var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);
            //view.Measure(msw, msh);
            //view.Layout(0, 0, r - l, b - t);



            //FormsViewPager pager = _viewPager;
            //Context context = Context;

            var width = r - l;
            var height = b - t;
            view.Layout(0, 0, width, height);
            //  _recyclerView.Layout(0, 0, 64, height);
            var widthPixels = TypedValue.ApplyDimension(ComplexUnitType.Dip,64, Context.Resources.DisplayMetrics);
            _cardView.Layout(0, 0, (int)widthPixels, height);
            _cardContainer.Layout(0, 0, (int) widthPixels, height);
            _viewPager.Layout((int)widthPixels, 0, (int)(width - 0), height);
            _viewPager.SetBackgroundColor(Colors.Thistle.ToAndroid());
            //_viewPager.SetBackgroundColor(Colors.DimGrey.ToAndroid());

            var headerMeasure = Element.Header.Measure(double.MaxValue, double.MaxValue);

            var headerHeight = headerMeasure.Request.Height != 0
                ? headerMeasure.Request.Height
                : headerMeasure.Minimum.Height;

            if (Element.HeaderHeight != 0)
            {
                headerHeight = (int)Element.HeaderHeight;
            }
            var headerHeightPx = TypedValue.ApplyDimension(ComplexUnitType.Dip, (float)headerHeight, Context.Resources.DisplayMetrics);

            _headerViewContainer.Layout(0, 0, (int)widthPixels, (int)headerHeightPx);

            var footerMeasure = Element.Footer.Measure(double.MaxValue, double.MaxValue);

            var footerHeight = footerMeasure.Request.Height != 0
                ? footerMeasure.Request.Height
                : footerMeasure.Minimum.Height;

            var footerHeightPx = TypedValue.ApplyDimension(ComplexUnitType.Dip, (float)footerHeight, Context.Resources.DisplayMetrics);

            var recyclerHeight = height - footerHeightPx - headerHeightPx;

            var footerOffset = headerHeightPx + recyclerHeight;

         //   _footerViewContainer.Layout(0, (int)footerOffset, (int)widthPixels, (int)footerHeightPx);
            _recyclerView.Layout(0, (int)headerHeightPx, (int)widthPixels, (int)recyclerHeight);
       
            base.OnLayout(changed, l, t, r, b);
        }

    }
}
