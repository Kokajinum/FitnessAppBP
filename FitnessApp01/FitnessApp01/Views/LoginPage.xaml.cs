using FitnessApp01.Helpers;
using FitnessApp01.Resx;
using FitnessApp01.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp01.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //if (!Connection.IsConnected)
            //{
            //    await DisplayAlert(AppResources.Error, AppResources.InternetRequired, "Ok");
            //}
            //else
            //{
            //    if (AuthBase.IsAuthenticated())
            //    {
            //        await Navigation.PushAsync(new AppShell());
            //    }
            //}

            //await AuthBase.SignOut();
            // pokud je zařízení offline a má platný token, necháme ho projít do diary
            if (AuthBase.IsAuthenticated())
            {
                await Shell.Current.GoToAsync("//DiaryPage");
            }
            else
            {
                // pokud nemá platný token, tak uživatele upozorníme na chybějící internet
                if (!Connection.IsConnected)
                {
                    await DisplayAlert(AppResources.Error, AppResources.InternetRequired, "Ok");
                }
            }
        }

        //private async void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        //{
        //    if (IsInternetAvailable())
        //    {
        //        if (AuthBase.IsAuthenticated())
        //        {
        //            await Shell.Current.GoToAsync("//main-content");
        //            Connectivity.ConnectivityChanged -= ConnectivityChanged;
        //        }
        //        else
        //        {
        //            await DisplayAlert(AppResources.Change, AppResources.InternetAvailable, "Ok");
        //        }
        //    }
        //    else
        //    {
        //        await DisplayAlert(AppResources.Error, AppResources.InternetRequired, "Ok");
        //    }
        //}



        //private bool IsInternetAvailable()
        //{
        //    return Connectivity.NetworkAccess == NetworkAccess.Internet;
        //}
    }
}