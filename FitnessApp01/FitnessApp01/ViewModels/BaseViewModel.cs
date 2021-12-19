using FitnessApp01.Interfaces;
using FitnessApp01.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FitnessApp01.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;

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
