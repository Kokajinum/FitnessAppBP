using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Graphics;
using Xamarin.Forms;
using FitnessApp01.Interfaces;
using Xamarin.Essentials;

[assembly: Dependency(typeof(FitnessApp01.Droid.MainActivity))]
namespace FitnessApp01.Droid
{
    [Activity(Label = "Fitness App", MainLauncher = false, Icon = "@drawable/ic_launcher_foreground", Theme = "@style/MainTheme", 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | 
        ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.Portrait )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IStatusBar
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// Nastaví barvu status baru
        /// </summary>
        /// <param name="hexColor">např. "#FFFFFF"</param>
        public void SetStatusBarColor(string hexColor)
        {
            var activity = Platform.CurrentActivity;
            activity.Window.SetStatusBarColor(Android.Graphics.Color.ParseColor(hexColor));
        }
    }
}