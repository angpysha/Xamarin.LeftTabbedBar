using System;
using System.Collections.Generic;
using Android.Runtime;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.RecyclerView.Widget;
using AndroidX.ViewPager2.Adapter;
using Microsoft.Maui.Essentials;
using Object = Java.Lang.Object;

namespace Plugin.Angpysha.LeftTabbedPage.Android.Controls
{
    public class LeftTabbedViewPagerAdapter : FragmentStateAdapter 
    {
        private int currentOffset = 0;
        private int nexOffset = 0;
        private List<LeftTabbedFragment> _fragments;

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

        public LeftTabbedViewPagerAdapter(FragmentManager fragmentManager, Lifecycle lifecycle,
             WeakReference renderer, List<LeftTabbedFragment> fragments) : base(fragmentManager, lifecycle)
        {
            _fragments = new();
            _weakRenderer = renderer;
            _fragments = fragments;
        }

        public LeftTabbedViewPagerAdapter(FragmentManager fragmentManager, Lifecycle lifecycle,
           WeakReference renderer) : base(fragmentManager, lifecycle)
        {
            _fragments = new();
            _weakRenderer = renderer;

        }

        public LeftTabbedViewPagerAdapter(FragmentActivity fragmentActivity, WeakReference renderer, List<LeftTabbedFragment> fragments) : this(fragmentActivity)
        {
            _fragments = new();
            _weakRenderer = renderer;
            _fragments = fragments;
        //    this.RegisterAdapterDataObserver(new LeftTabbedFragmentAdapterObserver(new WeakReference(this)));
        }

        private int _numPages;
        private WeakReference _weakRenderer;

        

        public override int ItemCount
        {
            get
            {
                return _fragments.Count;
            }
        }

        public override int GetItemViewType(int position)
        {
            return base.GetItemViewType(position);
        }

        public override bool ContainsItem(long itemId)
        {
            return base.ContainsItem(itemId);
        }

        public void NotifyToItems()
        {
            this.NotifyDataSetChanged();
           // this.NotifyItemInserted(_fragments.Count-1);
        }
        public override Fragment CreateFragment(int p0)
        {
            return _fragments[p0];
        }

        public override long GetItemId(int position)
        {
            return _fragments[position].GetHashCode()+currentOffset;
        }

        //public void AddFragments(List<Fragment> fragments)
        //{
        //    _fragments.AddRange(fragments);
        //    this.NotifyToItems();
        //}

        internal void AddFragments(List<LeftTabbedFragment> fragments)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {

                _fragments.AddRange(fragments);
                nexOffset = fragments.Count;
                this.NotifyToItems();
            });
        }

        public class LeftTabbedFragmentAdapterObserver : RecyclerView.AdapterDataObserver
        {
            private WeakReference _weakReference;
            private LeftTabbedViewPagerAdapter leftTabbedViewPagerAdapter => _weakReference.Target as LeftTabbedViewPagerAdapter;
            public LeftTabbedFragmentAdapterObserver(WeakReference weakReference) : base()
            {
                _weakReference = weakReference;
            }
            public override void OnChanged()
            {
                leftTabbedViewPagerAdapter.currentOffset = leftTabbedViewPagerAdapter.nexOffset;
                leftTabbedViewPagerAdapter.nexOffset += leftTabbedViewPagerAdapter.nexOffset;
            }
        }
    }
}