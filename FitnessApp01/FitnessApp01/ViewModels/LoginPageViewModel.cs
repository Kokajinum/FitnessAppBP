using FitnessApp01.Services;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FitnessApp01.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        public LoginPageViewModel()
        {
            RegisterLabelTapCommand = new Command(() => RegisterLabelTap());
            LoginLabelTapCommand = new Command(() => LoginLabelTap());

            LoginCommand = new Command(
                execute : async () => await Login(),
                canExecute: () => LoginCanExecute());

            RegisterCommand = new Command(
                execute: async () => await Register(),
                canExecute: () => RegisterCanExecute());

            SettingsStartCommand = new Command(async () => await SettingsStart());
        }

        private async Task SettingsStart()
        {
            await Shell.Current.GoToAsync("//RegistrationSettingsPage");
        }

        private bool RegisterCanExecute()
        {
            return CanRegister;
        }

        private async Task Register()
        {
            if ((UserConfirmPassword != UserPassword) || !ValidateEmail())
            {
                await DisplayAlertAsync("Error", "Hesla se neshodují", "ok");
                return;
            }
            try
            {
                new MailAddress(UserEmail);
            }
            catch (FormatException)
            {
                await DisplayAlertAsync("Error", "Neplatná emailová adresa", "ok");
                return;
            }
            
            bool result = await AuthBase.RegisterUserAsync(UserEmail, UserPassword);
            if (result) //registrovaný uživatel je zároveň přihlášený
            {
                LoginLabelTap();
                await GoToPageAsync("//WelcomePage");
                //await Shell.Current.GoToAsync("//WelcomePage");
                ResetFields();
            }

        }

        private bool LoginCanExecute()
        {
            return CanLogin;
        }

        private async Task Login()
        {
            bool result = await AuthBase.LoginUserAsync(UserEmail, UserPassword);
            if (result)
            {
                await GoToPageAsync("//main-content");
                ResetFields();
            }
        }

        private void LoginLabelTap()
        {
            LoginIsVisible = true;
            RegisterIsVisible = false;
        }

        private void RegisterLabelTap()
        {
            LoginIsVisible = false;
            RegisterIsVisible = true;
        }



        #region Helpers

        private void ResetFields()
        {
            UserEmail = string.Empty;
            UserPassword = string.Empty;
            UserConfirmPassword = string.Empty;
        }

        //TODO
        private bool ValidateEmail()
        {
            return true;
        }

        #endregion

        #region Properties

        private bool _loginIsVisible = true;
        public bool LoginIsVisible
        {
            get { return _loginIsVisible; }
            set { SetProperty(ref _loginIsVisible, value); }
        }

        private bool _registerIsVisible = false;
        public bool RegisterIsVisible
        {
            get { return _registerIsVisible; }
            set { SetProperty(ref _registerIsVisible, value); }
        }

        private string _userEmail = string.Empty;
        public string UserEmail
        {
            get { return _userEmail; }
            set
            {
                SetProperty(ref _userEmail, value);
                OnPropertyChanged("CanLogin");
                OnPropertyChanged("CanRegister");
            }
        }

        private string _userPassword = string.Empty;
        public string UserPassword
        {
            get { return _userPassword; }
            set
            {
                SetProperty(ref _userPassword, value);
                OnPropertyChanged("CanLogin");
                OnPropertyChanged("CanRegister");
            }
        }

        private string _userConfirmPassword = string.Empty;
        public string UserConfirmPassword
        {
            get { return _userConfirmPassword; }
            set
            {
                SetProperty(ref _userConfirmPassword, value);
                OnPropertyChanged("CanRegister");
            }
        }

        public bool CanLogin
        {
            get { return !string.IsNullOrEmpty(UserEmail) && !string.IsNullOrEmpty(UserPassword); }
        }

        public bool CanRegister
        {
            get
            {
                return !string.IsNullOrEmpty(UserEmail) && !string.IsNullOrEmpty(UserPassword) &&
                  !string.IsNullOrEmpty(UserConfirmPassword);
            }
        }


        #endregion

        #region Commands

        public ICommand RegisterLabelTapCommand { get; set; }
        public ICommand LoginLabelTapCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        
        public ICommand SettingsStartCommand { get; set; }

        #endregion
    }
}
