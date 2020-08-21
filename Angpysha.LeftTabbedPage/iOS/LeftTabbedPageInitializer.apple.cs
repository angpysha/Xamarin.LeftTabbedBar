using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Plugin.Angpysha.LeftTabbedPage.iOS
{
    public class LeftTabbedPageInitializer
    {
        public static void Init()
        {
             Debug.WriteLine($"{typeof(LeftTabbedPageRenderer).FullName} loaded");
             Debug.WriteLine($"{typeof(ViewCellExRenderer).FullName} loaded");
             Debug.WriteLine($"{typeof(UITabsView).FullName} loaded");

        }
    }
}
