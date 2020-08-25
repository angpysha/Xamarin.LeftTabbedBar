using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using PropertyChanged;

namespace Plugin.Angpysha.LeftTabbedPage.Shared
{
    [AddINotifyPropertyChangedInterface]
    public class MenuItem
    {

        public string Title { get; set; }

        public string Icon { get; set; }

        public ImageSource IconImageSource { get; set; }

        public bool Active { get;set; }
    }
}
