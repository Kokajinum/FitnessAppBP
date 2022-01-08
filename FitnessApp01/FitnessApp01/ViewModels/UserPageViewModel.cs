using FitnessApp01.Helpers;
using FitnessApp01.Models;
using FitnessApp01.Resx;
using FitnessApp01.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FitnessApp01.ViewModels
{
    public class UserPageViewModel : BaseViewModel
    {
        public UserPageViewModel()
        {
            SignOutCommand = new Command(async () => await SignOut());
            CaloriesGoalChangeCommand = new Command(async () => await CaloriesGoalChange());
            AgeChangeCommand = new Command(async () => await AgeChange());
            SettingsIconTapCommand = new Command(() => SettingsIconTap());
            UserIconTapCommand = new Command(() => UserIconTap());
            SaveChangesCommand = new Command(async () => await SaveChanges());
            GoalWeightChangeCommand = new Command(async () => await GoalWeightChange());
            HeightChangeCommand = new Command(async () => await HeightChange());
            ActivityChangeCommand = new Command(async () => await ActivityChange());
            SpeedChangeCommand = new Command(async () => await SpeedChange());
            PasswordChangeCommand = new Command(async () => await PasswordChange());
            ActualPasswordChangeCommand = new Command(async () => await ActualPasswordChange());
            AboutAppCommand = new Command(() => AboutApp());
            AboutAppCloseCommand = new Command(() => AboutAppClose());
            InitializeViewModelCommand = new Command(() => InitializeViewModel());

            
        }

        private void InitializeViewModel()
        {
            RegistrationSettings = Diary.RegistrationSettings;
        }

        private void AboutAppClose()
        {
            IsMainSLVisible = true;
            IsAboutAppVisible = false;
        }

        private void AboutApp()
        {
            IsMainSLVisible = false;
            IsAboutAppVisible = true;
        }

        #region Command implementace

        private async Task SaveChanges()
        {
            if (!Connection.IsConnected)
            {
                await DisplayErrorAsync(AppResources.InternetRequired);
                return;
            }
            if (!SomethingUnsaved)
            {
                await DisplayErrorAsync("Není co uložit.");
                return;
            }
            IsBusy = true;
            try
            {
                await FirestoreBase.UpdateRegistrationSettingsAsync(RegistrationSettings);
                MessagingCenter.Send<object>(this, "diaryUpdated");
                await DisplaySuccessAsync("Podařilo se aktualizovat registrační údaje.");
                EverythingSaved();
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync(ex.Message);
            }
            IsBusy = false;
        }

        private void UserIconTap()
        {
            IsSettingsIconVisible = true;
            IsUserIconVisible = false;
        }

        private void SettingsIconTap()
        {
            IsSettingsIconVisible = false;
            IsUserIconVisible = true;
        }

        private async Task CaloriesGoalChange()
        {
            string result = await DisplayPromptAsync("Manuální změna kalorií. Pouze pro zkušené uživatele!", Keyboard.Numeric, maxLength: 4);
            int parsedResult;
            if (int.TryParse(result, out parsedResult))
            {
                RegistrationSettings.CaloriesGoal = parsedResult;
                SomethingUnsavedChanged();
            }
            else if (parsedResult < 1000)
            {
                await DisplayErrorAsync("Změna se nepovedla. Hodnota musí být větší než 1000");
                return;
            }
            else
            {
                return;
            }
        }

        private async Task AgeChange()
        {
            string result = await DisplayPromptAsync("Změna věku", Keyboard.Numeric, maxLength: 2);
            int parsedResult;
            if (int.TryParse(result, out parsedResult))
            {
                RegistrationSettings.AgeDB = parsedResult;
                //po změně věku se musí přepočítat CaloriesGoal
                RegistrationSettings.CaloriesGoal = CalorieGoalCalculator.Calculate(RegistrationSettings);
                SomethingUnsavedChanged();
            }
            else
            {
                await DisplayErrorAsync("Změna se nepovedla");
            }
        }

        private async Task GoalWeightChange()
        {
            string result = await DisplayPromptAsync("Změna cílové váhy", Keyboard.Numeric, maxLength: 5);
            double parsedResult;
            if (double.TryParse(result, out parsedResult))
            {
                RegistrationSettings.DesiredWeightDB = parsedResult;
                //po změně se musí přepočítat CaloriesGoal
                RegistrationSettings.CaloriesGoal = CalorieGoalCalculator.Calculate(RegistrationSettings);
                SomethingUnsavedChanged();
            }
            else
            {
                await DisplayErrorAsync("Změna se nepovedla");
            }
        }

        private async Task HeightChange()
        {
            string result = await DisplayPromptAsync("Změna výšky", Keyboard.Numeric, maxLength: 5);
            double parsedResult;
            if (double.TryParse(result, out parsedResult))
            {
                RegistrationSettings.HeightDB = parsedResult;
                //po změně se musí přepočítat CaloriesGoal
                RegistrationSettings.CaloriesGoal = CalorieGoalCalculator.Calculate(RegistrationSettings);
                SomethingUnsavedChanged();
            }
            else
            {
                await DisplayErrorAsync("Změna se nepovedla");
            }
        }

        private async Task ActivityChange()
        {
            double activityValue = RegistrationSettings.ActivityDB;
            const string ans1 = "sedavé zaměstnání, sportování/cvičení maximálně 1x týdně";
            const string ans2 = "sportování/cvičení 2-3x týdně";
            const string ans3 = "sportování/cvičení 4-6x týdně";
            const string ans4 = "náročné sportování/cvičení každý den";
            string action = await App.Current.MainPage.DisplayActionSheet("Změna úrovně aktivity:",
                "zrušit", null, ans1, ans2, ans3, ans4);
            switch (action)
            {
                case ans1:
                    activityValue = 1.2;
                    break;
                case ans2:
                    activityValue = 1.375;
                    break;
                case ans3:
                    activityValue = 1.55;
                    break;
                case ans4:
                    activityValue = 1.725;
                    break;
            }
            RegistrationSettings.ActivityDB = activityValue;
            RegistrationSettings.CaloriesGoal = CalorieGoalCalculator.Calculate(RegistrationSettings);
            SomethingUnsavedChanged();
        }

        private async Task SpeedChange()
        {
            string title;
            if (RegistrationSettings.GoalDB == 1)
            {
                title = "Hubnout rychlostí: ";
            }
            else if (RegistrationSettings.GoalDB == 2)
            {
                title = "Přibírat rychlostí: ";
            }
            else
            {
                await DisplayErrorAsync("Nelze upravit rychlost.");
                return;
            }
            double speedValue = RegistrationSettings.GoalSpeed;
            const string ans1 = "0.2-0.25 kg/týden (výchozí)";
            const string ans2 = "0.4-0.5 kg/týden";
            const string ans3 = "0.6-0.7 kg/týden (nedoporučované)";
            string action = await App.Current.MainPage.DisplayActionSheet(title,
                "zrušit", null, ans1, ans2, ans3);
            switch (action)
            {
                case ans1:
                    speedValue = 0.1;
                    break;
                case ans2:
                    speedValue = 0.2;
                    break;
                case ans3:
                    speedValue = 0.3;
                    break;
                default:
                    return;
            }
            RegistrationSettings.GoalSpeed = speedValue;
            RegistrationSettings.CaloriesGoal = CalorieGoalCalculator.Calculate(RegistrationSettings);
            SomethingUnsavedChanged();
        }

        private async Task PasswordChange()
        {
            if (!Connection.IsConnected)
            {
                await DisplayErrorAsync(AppResources.InternetRequired);
                return;
            }
            await GoToPageAsync("PasswordChangePage");
        }

        private async Task SignOut()
        {
            bool result = await AuthBase.SignOut();
            if (result)
            {
                if (Diary.ClearData())
                {
                    MessagingCenter.Send<object>(this, "signedOut");
                }
                await GoToPageAsync("//LoginPage");
            }
        }

        private async Task ActualPasswordChange()
        {
            if (!CanSavePassword)
            {
                await DisplayErrorAsync(AppResources.CanNotSave);
                ClearPasswordProperties();
                return;
            } 
            else if (NewPassword != NewPasswordConfirm)
            {
                await DisplayErrorAsync(AppResources.PasswordConfirmError);
                ClearPasswordProperties();
                return;
            }
            else if (NewPassword == OldPassword)
            {
                await DisplayErrorAsync(AppResources.PasswordNotDifferent);
                ClearPasswordProperties();
                return;
            }
            try
            {
                await AuthBase.UpdatePassword(OldPassword, NewPassword);
                await DisplaySuccessAsync("Heslo bylo změněno.");
                await GoToPageAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync(ex.Message);
                ClearPasswordProperties();
            }
        }

        private void ClearPasswordProperties()
        {
            OldPassword = String.Empty;
            NewPassword = String.Empty;
            NewPasswordConfirm = String.Empty;
        }

        private void SomethingUnsavedChanged()
        {
            SaveButtonOpacity = 1;
            SomethingUnsaved = true;
        }

        private void EverythingSaved()
        {
            SaveButtonOpacity = 0.2;
            SomethingUnsaved = false;
        }

        #endregion

        #region Properties

        private string _userEmail;
        public string UserEmail
        {
            get { return _userEmail; }
            set { SetProperty(ref _userEmail, value); }
        }

        private RegistrationSettings _registrationSettings;
        public RegistrationSettings RegistrationSettings 
        { 
            get { return _registrationSettings; }
            set { SetProperty(ref _registrationSettings, value); }
        }



        private bool _isSettingsIconVisible = true;
        public bool IsSettingsIconVisible
        {
            get { return _isSettingsIconVisible; }
            set { SetProperty(ref _isSettingsIconVisible, value); }
        }

        private bool _isUserIconVisible = false;
        public bool IsUserIconVisible
        {
            get { return _isUserIconVisible; }
            set { SetProperty(ref _isUserIconVisible, value); }
        }

        private bool _somethingUnsaved;
        public bool SomethingUnsaved
        {
            get { return _somethingUnsaved; }
            set { SetProperty(ref _somethingUnsaved, value); }
        }

        private double _saveButtonOpacity = 0.2;
        public double SaveButtonOpacity
        {
            get { return _saveButtonOpacity; }
            set { SetProperty(ref _saveButtonOpacity, value); }
        }

        #region Passwd change

        private string _oldPassword;
        public string OldPassword
        {
            get { return _oldPassword; }
            set { SetProperty(ref _oldPassword, value); CanSavePasswordChanged(); }
        }

        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set { SetProperty(ref _newPassword, value); CanSavePasswordChanged(); }
        }

        private string _newPasswordConfirm;
        public string NewPasswordConfirm
        {
            get { return _newPasswordConfirm; }
            set { SetProperty(ref _newPasswordConfirm, value); CanSavePasswordChanged(); }
        }

        private double _passwordButtonOpacity = 0.2;
        public double PasswordButtonOpacity
        {
            get { return _passwordButtonOpacity; }
            set { SetProperty(ref _passwordButtonOpacity, value); }
        }

        public bool CanSavePassword
        {
            get
            {
                return !string.IsNullOrEmpty(OldPassword) 
                    && !string.IsNullOrEmpty(NewPasswordConfirm) 
                    && !string.IsNullOrEmpty(NewPassword);
            }
        }

        private void CanSavePasswordChanged()
        {
            if (CanSavePassword)
            {
                PasswordButtonOpacity = 1;
            }
            else
            {
                PasswordButtonOpacity = 0.2;
            }
        }

        #endregion

        #region AboutApp

        private bool _isMainSLVisible = true;
        public bool IsMainSLVisible
        {
            get { return _isMainSLVisible;}
            set { SetProperty(ref _isMainSLVisible, value); }
        }

        private bool _isAboutAppVisible = false;
        public bool IsAboutAppVisible
        {
            get { return _isAboutAppVisible; }
            set { SetProperty(ref _isAboutAppVisible, value); }
        }

        #endregion

        #endregion

        #region Commands 

        public ICommand SignOutCommand { get; set; }
        public ICommand CaloriesGoalChangeCommand { get; set; }
        public ICommand AgeChangeCommand { get; set; }
        public ICommand SettingsIconTapCommand { get; set; }
        public ICommand UserIconTapCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }
        public ICommand GoalWeightChangeCommand { get; set; }
        public ICommand HeightChangeCommand { get; set; }
        public ICommand ActivityChangeCommand { get; set; }
        public ICommand SpeedChangeCommand { get; set; }
        public ICommand PasswordChangeCommand { get; set; }
        public ICommand ActualPasswordChangeCommand { get; set; }
        public ICommand AboutAppCommand { get; set; }
        public ICommand AboutAppCloseCommand { get; set; }
        public ICommand InitializeViewModelCommand { get; set; }

        #endregion
    }
}
