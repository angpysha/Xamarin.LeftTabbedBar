using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Angpysha.LeftTabbedPage.Demo.Services;
using Angpysha.LeftTabbedPage.Demo.Views;
using Xamarin.Essentials;

namespace Angpysha.LeftTabbedPage.Demo
{
    public partial class App : Application
    {
        public static double PageMaxWidth = DeviceDisplay.MainDisplayInfo.Width/DeviceDisplay.MainDisplayInfo.Density - 64;
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
