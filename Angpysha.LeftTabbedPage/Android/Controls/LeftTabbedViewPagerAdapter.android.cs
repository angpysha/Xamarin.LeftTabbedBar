using System;
using System.Collections.Generic;
using Android.Runtime;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.ViewPager2.Adapter;
using Xamarin.Forms;

namespace Plugin.Angpysha.LeftTabbedPage.Android.Controls
{
    public class LeftTabbedViewPagerAdapter : FragmentStateAdapter 
    {
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

        public LeftTabbedViewPagerAdapter(FragmentActivity fragmentActivity, List<Page> pages) : this(fragmentActivity)
        {
            _numPages = pages.Count;
            _pages = pages;
        }

        private int _numPages;
        private List<Page> _pages;

        public override int ItemCount => _numPages;
        public override Fragment CreateFragment(int p0)
        {
            return new LeftTabbedFragment(_pages[p0]);
        }
    }
}