using FitnessApp01.Helpers;
using FitnessApp01.Interfaces;
using FitnessApp01.Services;
using FitnessApp01.Views;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FitnessApp01
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            Connection.SubscribeForChanges();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            SetFontSize();
            SetStatusBarColor();
        }

        private void SetStatusBarColor()
        {
            var activity = DependencyService.Get<IStatusBar>();
            OSAppTheme currentTheme = Application.Current.RequestedTheme;
            if ((currentTheme == OSAppTheme.Dark) || (currentTheme == OSAppTheme.Unspecified))
            {
                //var color = Application.Current.Resources
                //    .Where(x => x.Key.Equals("TabbarColor")).FirstOrDefault().Value;
                activity.SetStatusBarColor("#00171F");
            }
            else
            {
                activity.SetStatusBarColor("#3E92CC");
            }
        }

        /// <summary>
        /// úprava fontů pro rozlišení FHD a menší
        /// </summary>
        private void SetFontSize()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var width = mainDisplayInfo.Width;
            var height = mainDisplayInfo.Height;
            if (width <= 1080 && height <= 1920)
            {
                //úprava stylů v App.xaml
                //funguje v případě, že dodržujeme pravidlo, že 1. je setter FontSizu, jinak použít foreach
                SettingsLabel.Setters[0].Value = 15;
                MacronutrientsLabel.Setters[0].Value = 13;
                AddFoodEntry.Setters[0].Value = 13;
                //foreach (var i in SettingsLabel.Setters)
                //{
                //    if (i.Property.PropertyName == "FontSize")
                //    {
                //        i.Value = 15;
                //    }
                //}
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
