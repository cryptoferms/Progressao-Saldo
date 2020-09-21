using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleSaldoCircular
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new CircularGreetingPage();
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
