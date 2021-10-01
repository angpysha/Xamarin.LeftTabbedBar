using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Plugin.Angpysha.LeftTabbedPage.iOS
{
    public class LeftTabbedPageInitializer
    {
        public static void Init(MauiAppBuilder appBuilder)
        {
            appBuilder.ConfigureMauiHandlers(c =>
            {
                c.AddCompatibilityRenderer(typeof(Shared.LeftTabbedPage), typeof(LeftTabbedPageRenderer));
            });
             //Debug.WriteLine($"{typeof(LeftTabbedPageRenderer).FullName} loaded");
             //Debug.WriteLine($"{typeof(ViewCellExRenderer).FullName} loaded");
             //Debug.WriteLine($"{typeof(UITabsView).FullName} loaded");

        }
    }
}
