using Android.Content;
using Android.OS;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Angpysha.LeftTabbedPage.Android
{
    public static class LeftTabbedPageInitializer
    {
        public static void Init(MauiAppBuilder appBuilder)
        {
            appBuilder.ConfigureMauiHandlers(c =>
            {
                c.AddCompatibilityRenderer(typeof(Shared.LeftTabbedPage), typeof(LeftTabbedPageRenderer));
            });
            System.Diagnostics.Debug.WriteLine($"{typeof(LeftTabbedPageRenderer).FullName} loaded");
            //Debug.WriteLine($"{typeof(ViewCellExRenderer).FullName} loaded");
            //Debug.WriteLine($"{typeof(UITabsView).FullName} loaded");
        }
    }
}
