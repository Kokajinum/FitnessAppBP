using FitnessApp01.Views;
using Plugin.CloudFirestore;
using Xamarin.Forms;

namespace FitnessApp01
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            //MainPage = new LoginPage();
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
