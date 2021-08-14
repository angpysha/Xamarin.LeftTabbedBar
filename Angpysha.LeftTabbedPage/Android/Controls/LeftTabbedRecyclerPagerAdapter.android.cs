using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Text;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Xamarin.Forms;
using Context = Android.Content.Context;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using Object = Java.Lang.Object;
using View = Android.Views.View;

namespace Plugin.Angpysha.LeftTabbedPage.Android.Controls
{
    public class LeftTabbedRecyclerPagerAdapter : RecyclerView.Adapter
    {
        public WeakReference<Shared.LeftTabbedPage> weakTabbed;

        public Context Context;
        public List<Page> Pages;

        public LeftTabbedRecyclerPagerAdapter(List<Page> pages, WeakReference<Shared.LeftTabbedPage> tabbed,
            Context context)
        {
            Pages = pages;
            weakTabbed = tabbed;
            Context = context;
        }

        public override int ItemCount => Pages?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            int iiii = 0;
        }

        private int posiiton = 0;


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (weakTabbed.TryGetTarget(out var tabbed))
            {
                var _page = Pages[posiiton];
                var dm = Context.Resources.DisplayMetrics;



                var screenWidthPixels = dm.WidthPixels;
                var tabWidthPixes = TypedValue.ApplyDimension(ComplexUnitType.Dip, 64, dm);
                var newWidthPixels = screenWidthPixels - tabWidthPixes;
                var newWidth = pxToDp((int)newWidthPixels, dm);
                var heightPixels = dm.HeightPixels;
                var height = pxToDp(heightPixels, dm);

                
                 var formsPageRenderer = Platform.CreateRendererWithContext(_page, Context);
                 var aView = formsPageRenderer.View;
                 formsPageRenderer.Tracker.UpdateLayout();
                 var lp = new ViewGroup.LayoutParams(128,128);
                 aView.LayoutParameters = lp;
                 _page.Layout(new Rectangle(0,0,128,128));
                 posiiton++;
                 return new RecyclerViewMenuItemViewHolder(aView);
            }

            return null;
        }

        public void OnItemChanged(int i)
        {
            NotifyItemChanged(i);
        }

        public int pxToDp(int px, DisplayMetrics dm)
        {

            return (int)Math.Round(px / (dm.Xdpi / (double)global::Android.Util.DisplayMetricsDensity.Default));
        }
    }

    public class LeftTabbedRecyclerPagerViewHolder : RecyclerView.ViewHolder
    {
        public LeftTabbedRecyclerPagerViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public LeftTabbedRecyclerPagerViewHolder(View itemView) : base(itemView)
        {
        }

        
    }
}
