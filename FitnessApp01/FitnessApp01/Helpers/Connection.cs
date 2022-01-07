using FitnessApp01.Resx;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace FitnessApp01.Helpers
{
    /// <summary>
    /// Pomocná třída ohledně připojení k internetu, zapouzdřuje Essentials.Connectivity
    /// </summary>
    public static class Connection
    {
        /// <summary>
        /// Zjistí, zda je zařízení připojeno k internetu
        /// </summary>
        public static bool IsConnected
        {
            get 
            {
                return Connectivity.NetworkAccess == NetworkAccess.Internet;
            }
        }

        /// <summary>
        /// Zapne oznamování při změně připojení (DisplayAlert)
        /// </summary>
        public static void SubscribeForChanges()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        /// <summary>
        /// Vypne oznamování při změně připojení 
        /// </summary>
        public static void UnsubscribeForChanged()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        private static async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Change, AppResources.InternetAvailable, "Ok");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Change, AppResources.InternetRequired, "Ok");
            }
        }
    }
}
