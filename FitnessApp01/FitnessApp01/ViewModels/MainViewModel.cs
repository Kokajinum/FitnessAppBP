using FitnessApp01.Models;
using FitnessApp01.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FitnessApp01.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            if (!IsLoaded)
                InitializeMainViewModel();
            SignOutCommand = new Command(SignOut);
        }

        private void InitializeMainViewModel()
        {
            LoadRegistrationSettingsAsync();
            IsLoaded = true;
        }

        private async void LoadRegistrationSettingsAsync()
        {
            RegistrationSettings = await FirestoreBase.LoadRegistrationSettings();
        }

        private async void SignOut(object obj)
        {
            bool result = await AuthBase.SignOut();
            if (result)
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }

        #region Properties

        private RegistrationSettings _registrationSettings;
        public RegistrationSettings RegistrationSettings
        {
            get { return _registrationSettings; }
            set { SetProperty(ref _registrationSettings, value); } 
        }

        public bool IsLoaded { get; set; } = false;

        #endregion

        #region Commands 

        public ICommand SignOutCommand { get; set; }

        #endregion
    }
}
