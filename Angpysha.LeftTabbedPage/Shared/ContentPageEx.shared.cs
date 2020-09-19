using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Plugin.Angpysha.LeftTabbedPage.Shared
{
    public class ContentPageEx : ContentPage
    {
        public ContentPageEx() : base()
        {
            if (WidthRequest == -1)
            {
                var width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density - 64;
                WidthRequest = width;
            }

            ChildAdded += OnChildAdded;
        }

        private void OnChildAdded(object sender, ElementEventArgs e)
        {
            var ele = e.Element as View;
            var margins = ele.Margin;
            var width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density - 64 - margins.Left - margins.Right;
           // WidthRequest = width;
            ele.WidthRequest = width;
            ele.HorizontalOptions = LayoutOptions.Start;
            
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var width = DeviceDisplay.MainDisplayInfo.Width;
            var widthNew = widthConstraint;
            widthNew = width - 64;
            return base.OnMeasure(widthNew, heightConstraint);
        }
    }
}
