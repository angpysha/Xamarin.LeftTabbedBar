using Android.Content;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Angpysha.LeftTabbedPage.Android
{
    public static class LeftTabbedPageInitializer
    {
        public static void Init(Context context, Bundle bundle)
        {
            
            System.Diagnostics.Debug.WriteLine($"{typeof(LeftTabbedPageRenderer).FullName} loaded");
            //Debug.WriteLine($"{typeof(ViewCellExRenderer).FullName} loaded");
            //Debug.WriteLine($"{typeof(UITabsView).FullName} loaded");
        }
    }
}
