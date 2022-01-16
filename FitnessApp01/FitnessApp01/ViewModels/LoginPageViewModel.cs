using FitnessApp01.Helpers;
using FitnessApp01.Resx;
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
                execute : async () => await Login());

            RegisterCommand = new Command(
                execute: async () => await Register());

            PasswordResetCommand = new Command(
                execute: async () => await PasswordReset());

            ActualPasswordResetCommand = new Command(
                execute: async () => await ActualPasswordReset());

            SettingsStartCommand = new Command(async () => await SettingsStart());
        }

        private async Task ActualPasswordReset()
        {
            if(!Connection.IsConnected)
            {
                await DisplayErrorAsync(AppResources.InternetRequired);
                return;
            }
            if (string.IsNullOrEmpty(ResetEmail))
            {
                await DisplayErrorAsync(AppResources.InvalidEmail);
                return;
            }
            try
            {
                new MailAddress(ResetEmail);
            }
            catch (FormatException)
            {
                await DisplayErrorAsync(AppResources.InvalidEmail);
                return;
            }
            try
            {
                await AuthBase.ResetPassword(ResetEmail);
                await DisplaySuccessAsync(AppResources.Success + ": " +
                    AppResources.CheckEmail);
                ResetEmail = string.Empty;
                await GoToPageAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync(ex.Message);
            }
        }

        private async Task PasswordReset()
        {
            await GoToPageAsync("PasswordResetPage");
        }

        private async Task SettingsStart()
        {
            await Shell.Current.GoToAsync("//RegistrationSettingsPage");
        }

        public async Task Register()
        {
            if (!Connection.IsConnected)
            {
                await DisplayErrorAsync(AppResources.InternetRequired);
                return;
            }
            if (!CanRegister)
            {
                await DisplayAlertAsync(AppResources.Error, AppResources.LoginPage_NoEmailOrPassword, "Ok");
                return;
            }
            if ((UserConfirmPassword != UserPassword) || !ValidateEmail())
            {
                await DisplayAlertAsync(AppResources.Error, AppResources.PasswordConfirmError, "Ok");
                return;
            }
            try
            {
                new MailAddress(UserEmail);
            }
            catch (FormatException)
            {
                await DisplayAlertAsync(AppResources.Error, AppResources.InvalidEmail, "Ok");
                return;
            }
            try
            {
                IsBusy = true;
                //registrovaný uživatel je zároveň přihlášený
                await AuthBase.RegisterUserAsync(UserEmail, UserPassword);
                LoginLabelTap();
               // await App.Current.MainPage.Navigation.PushAsync(new AppShell());
                await GoToPageAsync("//WelcomePage");
                ResetFields();
                IsBusy = false;
            }
            catch(Exception ex)
            {
                await DisplayAlertAsync(AppResources.Error, ex.Message, "Ok");

            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task Login()
        {
            if (!Connection.IsConnected)
            {
                await DisplayErrorAsync(AppResources.InternetRequired);
                return;
            }
            if (!CanLogin)
            {
                await DisplayAlertAsync(AppResources.Error, AppResources.LoginPage_NoEmailOrPassword, "Ok");
                return;
            }
            try
            {
                new MailAddress(UserEmail);
            }
            catch (FormatException)
            {
                await DisplayAlertAsync(AppResources.Error, AppResources.InvalidEmail, "Ok");
                return;
            }
            try
            {
                IsBusy = true;
                await AuthBase.LoginUserAsync(UserEmail, UserPassword);
                //await GoToPageAsync("//main-content");
                await GoToPageAsync("//DiaryPage");
                //App.Current.MainPage = new AppShell();
                //await App.Current.MainPage.Navigation.PushAsync(new AppShell());
                ResetFields();
                
            }
            catch(Exception ex)
            {
                await DisplayAlertAsync(AppResources.Error, ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
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

        private void CanLoginChanged()
        {
            if (CanLogin)
            {
                LoginButtonOpacity = 1;
            }
            else
            {
                LoginButtonOpacity = 0.2;
            }
        }

        private void CanRegisterChanged()
        {
            if (CanRegister)
            {
                RegisterButtonOpacity = 1;
            }
            else
            {
                RegisterButtonOpacity = 0.2;
            }
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
                CanLoginChanged();
                CanRegisterChanged();
            }
        }

        private string _userPassword = string.Empty;
        public string UserPassword
        {
            get { return _userPassword; }
            set
            {
                SetProperty(ref _userPassword, value);
                CanLoginChanged();
                CanRegisterChanged();
            }
        }

        

        private string _userConfirmPassword = string.Empty;
        public string UserConfirmPassword
        {
            get { return _userConfirmPassword; }
            set
            {
                SetProperty(ref _userConfirmPassword, value);
                CanRegisterChanged();
            }
        }

        private double _loginButtonOpacity = 0.2;
        public double LoginButtonOpacity
        {
            get { return _loginButtonOpacity; }
            set { SetProperty(ref _loginButtonOpacity, value); }
        }

        private double _registerButtonOpacity = 0.2;
        public double RegisterButtonOpacity
        {
            get { return _registerButtonOpacity; }
            set { SetProperty(ref _registerButtonOpacity, value); }
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

        private string _resetEmail;
        public string ResetEmail
        {
            get { return _resetEmail; }
            set { SetProperty(ref _resetEmail, value); }
        }


        #endregion

        #region Commands

        public ICommand RegisterLabelTapCommand { get; set; }
        public ICommand LoginLabelTapCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand SettingsStartCommand { get; set; }
        public ICommand PasswordResetCommand { get; set; }
        public ICommand ActualPasswordResetCommand { get; set; }

        #endregion
    }
}
