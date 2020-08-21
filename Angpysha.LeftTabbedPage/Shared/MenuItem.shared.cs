using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Plugin.Angpysha.LeftTabbedPage.Shared
{
    public class MenuItem : INotifyPropertyChanged
    {

        public string Title { get; set; }

        public string Icon { get; set; }

        public ImageSource IconImageSource { get; set; }

        public bool Active { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
