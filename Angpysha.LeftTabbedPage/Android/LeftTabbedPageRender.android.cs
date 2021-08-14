using Android.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
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
using Xamarin.Essentials;
using Platform = Xamarin.Forms.Platform.Android.Platform;

[assembly: ExportRenderer(typeof(LeftTabbedPage), typeof(LeftTabbedPageRenderer))]
namespace Plugin.Angpysha.LeftTabbedPage.Android
{
    public class LeftTabbedPageRenderer : VisualElementRenderer<Shared.LeftTabbedPage>,
        IPlatformElementConfiguration<Xamarin.Forms.PlatformConfiguration.Android, Shared.LeftTabbedPage>
    {
        private bool inited = false;
        internal global::Android.Views.View view;
        private ViewPager2 _viewPager;


        internal RecyclerView _recyclerView;
        internal RecyclerView _recyclerViewPager;
        private RecyclerViewAdapter _tabAdapter;
        private FrameLayout _headerViewContainer;
        private FrameLayout _footerViewContainer;


        public int CurrentIndex { get; set; }
        public int PreviousIndex { get; set; } = -1;

        public LeftTabbedPageRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Xamarin.Forms.TabbedPage.CurrentPage))
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
                var inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                view = inflater.Inflate(Resource.Layout.activity_main_android, null, false);
                view.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
                AddView(view);

                // lefttabbed_recycler
                _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.lefttabbed_recycler);
                _viewPager = view.FindViewById<ViewPager2>(Resource.Id.lefttabbed_viewpager);
                _headerViewContainer = view.FindViewById<FrameLayout>(Resource.Id.lefttabed_tabbar_header);
                _footerViewContainer = view.FindViewById<FrameLayout>(Resource.Id.lefttabed_tabbar_footer);
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
            // _recyclerView.BackgroundColor = C
            _recyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _recyclerView.SetAdapter(_tabAdapter);
            _tabAdapter.OnItemClicked += AdapterOnOnItemClicked;
            if (Context.GetActivity() is FragmentActivity fragmentActivity)
            {
                _leftTabbedViewPagerAdapter = new LeftTabbedViewPagerAdapter(fragmentActivity, new WeakReference(this));
                Fragments = Element.Children.Select(x => new LeftTabbedFragment(x, new WeakReference(this))).ToList();
                _leftTabbedViewPagerAdapter.AddFragments(Fragments);

                _viewPager.Adapter = _leftTabbedViewPagerAdapter;
                _viewPager.UserInputEnabled = false;
                _viewPager.RegisterOnPageChangeCallback(new PageChangedCallback(new WeakReference(this)));
                _viewPager.SetPageTransformer(null);

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

                //  Element.Header.Layout(new Rectangle(0,0,width, height));
                var dm = Context.Resources.DisplayMetrics;
                var renderer = Platform.CreateRendererWithContext(Element.Header, Context);
                renderer.Element.Layout(new Rectangle(0, 0, width, height));
                var widthPx = TypedValue.ApplyDimension(ComplexUnitType.Dip, width, dm);
                var heightPx = TypedValue.ApplyDimension(ComplexUnitType.Dip, (float)height, dm);
                var lp = new LayoutParams((int)widthPx, (int)heightPx);
                renderer.View.LayoutParameters = lp;
                //  renderer.Tracker.UpdateLayout();

                _headerViewContainer.AddView(renderer.View);
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
                var renderer = Platform.CreateRendererWithContext(Element.Footer, Context);
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
                    if (_skip)
                    {
                        _skip = false;
                        return;
                    }
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

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (inited)
                return;

            inited = true;
            //base.OnLayout(changed, l, t, r, b);
            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);
            view.Measure(msw, msh);
            view.Layout(0, 0, r - l, b - t);
        }

    }
}
