using GAI_Client.Services;
using GAI_Client.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GAI_Client
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
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
