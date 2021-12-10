using FitnessApp01.Interfaces;
using FitnessApp01.Services;
using Xamarin.Forms;

namespace FitnessApp01
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            //DependencyService.Register<IDatabase,FirestoreBase>();

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
