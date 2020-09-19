using Android.Content;
using System;
using System.Collections.Generic;
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
using Plugin.Angpysha.LeftTabbedPage.Android.Controls;

[assembly: ExportRenderer(typeof(LeftTabbedPage),typeof(LeftTabbedPageRenderer))]
namespace Plugin.Angpysha.LeftTabbedPage.Android
{
    public class LeftTabbedPageRenderer : VisualElementRenderer<Shared.LeftTabbedPage>, IPlatformElementConfiguration<Xamarin.Forms.PlatformConfiguration.Android, Shared.LeftTabbedPage>
    {
        private bool inited = false;
        private global::Android.Views.View view;


        private RecyclerView _recyclerView;
        public LeftTabbedPageRenderer(Context context) : base(context)
        {
           
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

               // var adapter = new RecyclerViewAdapter(me)
                SetMenuItems();
            }
        }

        private void SetMenuItems()
        {
            var menuitems = Element.Children.Select(x => new Shared.MenuItem()
            {
                Title = x.Title,
                IconImageSource = x.IconImageSource
            }).ToList();
            var adapter = new RecyclerViewAdapter(menuitems,
                new WeakReference<LeftTabbedPage.Shared.LeftTabbedPage>(Element), Context);
           // _recyclerView.BackgroundColor = C
           _recyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _recyclerView.SetAdapter(adapter);
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
