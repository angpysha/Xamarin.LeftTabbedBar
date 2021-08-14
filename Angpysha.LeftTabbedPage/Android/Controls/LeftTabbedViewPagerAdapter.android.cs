using System;
using System.Collections.Generic;
using Android.Runtime;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.RecyclerView.Widget;
using AndroidX.ViewPager2.Adapter;
using Xamarin.Forms;
using Object = Java.Lang.Object;

namespace Plugin.Angpysha.LeftTabbedPage.Android.Controls
{
    public class LeftTabbedViewPagerAdapter : FragmentStateAdapter 
    {
        private List<Fragment> _fragments;

        public LeftTabbedViewPagerAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public LeftTabbedViewPagerAdapter(Fragment fragment) : base(fragment)
        {
        }

        public LeftTabbedViewPagerAdapter(FragmentActivity fragmentActivity) : base(fragmentActivity)
        {
        }

        public LeftTabbedViewPagerAdapter(FragmentManager fragmentManager, Lifecycle lifecycle) : base(fragmentManager, lifecycle)
        {
        }

        public LeftTabbedViewPagerAdapter(FragmentActivity fragmentActivity, WeakReference renderer) : this(fragmentActivity)
        {
            _fragments = new();
            _weakRenderer = renderer;
        }

        private int _numPages;
        private WeakReference _weakRenderer;

        

        public override int ItemCount => _fragments.Count;

        public void NotifyToItems()
        {
            this.NotifyItemInserted(_fragments.Count-1);
        }
        public override Fragment CreateFragment(int p0)
        {
            return _fragments[p0];
        }

        public override long GetItemId(int position)
        {
            return _fragments[position].GetHashCode();
        }

        public void AddFragments(List<Fragment> fragments)
        {
            _fragments.AddRange(fragments);
            this.NotifyToItems();
        }

        internal void AddFragments(List<LeftTabbedFragment> fragments)
        {
            _fragments.AddRange(fragments);
            this.NotifyToItems();
        }
    }
}