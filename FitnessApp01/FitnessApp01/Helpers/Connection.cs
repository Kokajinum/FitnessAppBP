using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace FitnessApp01.Helpers
{
    /// <summary>
    /// Třída zjišťuje, zda je zařízení připojeno k internetu
    /// </summary>
    public static class Connection
    {
        public static bool IsConnected
        {
            get { return Connectivity.NetworkAccess == NetworkAccess.Internet; }
        }
    }
}
