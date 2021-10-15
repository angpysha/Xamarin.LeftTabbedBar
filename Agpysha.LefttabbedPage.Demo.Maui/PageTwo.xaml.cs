using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;

namespace Agpysha.LefttabbedPage.Demo.Maui
{
    public partial class PageTwo : ContentPage
    {
        int count = 0;

        public PageTwo()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
        }
    }
}
