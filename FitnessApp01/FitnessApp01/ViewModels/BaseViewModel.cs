using FitnessApp01.Interfaces;
using FitnessApp01.Models;
using FitnessApp01.Resx;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FitnessApp01.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifikuje o změně property, nastavuje property a zároveň kontroluje rovnost
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual async Task DisplayAlertAsync(string title, string message, string cancel)
        {
            await App.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        protected virtual void DisplayAlert(string title, string message, string cancel)
        {
            App.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        /// <summary>
        /// DisplayAlert s předvyplněným title a cancel pro oznámení chyby.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual async Task DisplayErrorAsync(string message)
        {
            await App.Current.MainPage.DisplayAlert(AppResources.Error, message, "Ok");
        }

        /// <summary>
        /// DisplayAlert s předvyplněný title a cancel pro oznámení úspěšné operace.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual async Task DisplaySuccessAsync(string message)
        {
            await App.Current.MainPage.DisplayAlert(AppResources.Success, message, "Ok");
        }

        protected virtual async Task<string> DisplayPromptAsync(string message, Keyboard keyboard, int maxLength)
        {
            return await App.Current.MainPage.DisplayPromptAsync
                (title: AppResources.SettingsChange, message: message, keyboard: keyboard, maxLength: maxLength);
        }

        protected virtual async Task GoToPageAsync(string shellPagePath)
        {
            await Shell.Current.GoToAsync(shellPagePath);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { SetProperty(ref _isBusy, value); }
        }

        protected IDatabase FirestoreBase { get; set; } = Services.FirestoreBase.Instance;
    }
}
