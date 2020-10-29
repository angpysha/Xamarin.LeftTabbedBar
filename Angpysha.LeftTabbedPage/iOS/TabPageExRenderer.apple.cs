using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Angpysha.LeftTabbedPage.iOS;
using Plugin.Angpysha.LeftTabbedPage.Shared;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(TabPage),typeof(TabPageExRenderer))]
namespace Plugin.Angpysha.LeftTabbedPage.iOS
{
    public class TabPageExRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var frame = View.Frame;

            var width = DeviceDisplay.MainDisplayInfo.Width/DeviceDisplay.MainDisplayInfo.Density;

            var width2 = frame.Width;

            ///int ii = 0;
            if (width2 == width)
            {
                var bounds = Element.Bounds;
                var ee = bounds.Width;
                bounds.Width = width2;

                Element.Layout(bounds);
                frame.Width = width2;
                View.Frame = frame;
                View.LayoutSubviews();
                View.SizeToFit();
            }
        }

        //private void ResizeSubviews(float width, UIView view)
        //{
        //    var frame = 
        //    if (view.Subviews?.Length > 0)
        //    {
        //        foreach (var viewSubview in view.Subviews)
        //        {
                    
        //        }
        //    }
        //    else
        //    {
                
        //    }
        //}
        
    }
}
