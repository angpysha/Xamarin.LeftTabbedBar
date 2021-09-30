using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace Plugin.Angpysha.LeftTabbedPage.UWP
{
    public class LeftTabbedPageInitializer
    {
        public static void Initialize()
        {
            Application.Current.Resources.MergedDictionaries.Add(new Resources());
        }
    }
}
