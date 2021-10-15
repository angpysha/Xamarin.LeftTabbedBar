using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;

namespace Agpysha.LefttabbedPage.Demo.Maui
{
    public partial class PageOne : ContentPage
    {
        int count = 0;

        public PageOne()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
            
        }
    }
}
