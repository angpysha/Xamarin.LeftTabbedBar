using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Maui.Controls;
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
