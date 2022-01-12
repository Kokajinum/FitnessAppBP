using FitnessApp01.Interfaces;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FitnessApp01.Services
{
    public class AuthBase
    {
        private static readonly IAuth auth = DependencyService.Get<IAuth>();

        public static async Task RegisterUserAsync(string email, string password)
        {
            try
            {
                await auth.RegisterUserAsync(email, password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task LoginUserAsync(string email, string password)
        {
            try
            {
                await auth.LoginUserAsync(email, password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task UpdatePassword(string oldPassword, string newPassword)
        {
            try
            {
                await auth.UpdatePassword(oldPassword, newPassword);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsAuthenticated()
        {
            return auth.IsAuthenticated();
        }

        public static string GetUserId()
        {
            return auth.GetUserId();
        }

        public static string GetUserEmail()
        {
            return auth.GetUserEmail();
        }

        public static async Task<bool> SignOut()
        {
            try
            {
                return auth.SignOut();
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.Message, "ok");
                return false;
            }
        }

        public static async Task ResetPassword(string email)
        {
            try
            {
                await auth.ResetPassword(email);
            }
            catch(Exception)
            {
                throw;
            }
        }
        
    }
}
