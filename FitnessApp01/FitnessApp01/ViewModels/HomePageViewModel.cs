using FitnessApp01.Helpers;
using FitnessApp01.Interfaces;
using FitnessApp01.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FitnessApp01.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public HomePageViewModel()
        {
            RefreshViewCommand = new Command(() => Refresh());
            InitializeViewModel = new Command(async () => await InitializeHomePageViewModel());

            HomePageAttributes = new HomePageAttributes();
            MessagingCenter.Subscribe<object>(this, "diaryUpdated", (p) =>
            {
                OnMessageReceived();
            });
        }

        private void Refresh()
        {
            SetDiaryData();
            HomePageAttributes.IsRefreshing = false;
        }

        private void OnMessageReceived()
        {
            IsBusy = true;
            SetDiaryData();
            IsBusy = false;
        }

        private async Task InitializeHomePageViewModel()
        {
            IsBusy = true;
            if (RegistrationSettings == null)
            {
                var isValid = await LoadRegistrationSettings();
                if (!isValid)
                {
                    await App.Current.MainPage
                        .DisplayAlert("Error", "nedokončená registrace - prosím znovu vyplňtě údaje", "ok");
                    await Shell.Current.GoToAsync("//RegistrationSettingsPage");
                }
            }
            SetDiaryData();
            IsBusy = false;
        }

        private async Task<bool> LoadRegistrationSettings()
        {
            RegistrationSettings = await FirestoreBase.ReadRegistrationSettingsAsync();
            return CheckRegistrationSettings();
        }

        /*private async Task<bool> LoadAndSetDiaryData()
        {
            Diary.Days = await FirestoreBase.LoadAndSetDiaryData();
            return SetDiaryData();
        }*/

        private bool SetDiaryData()
        {
            const int proteinGramKcal = 4; //1 gram == 4kcal
            const int carbohydratesGramKcal = 4;
            const int fatGramKcal = 9;
            if (Diary.Days.Count != 0)
            {
                Day it;
                Console.WriteLine(SelectedDay.ToUnixSecondsString());
                try
                {
                    it = Diary.Days.First(x => x.UnixSeconds == SelectedDay.ToUnixSecondsString());
                }
                catch(InvalidOperationException)
                {
                    it = Diary.Days.First();
                }
                HomePageAttributes.CaloriesGoal = RegistrationSettings.CaloriesGoal;
                HomePageAttributes.CaloriesCurrent = it.CaloriesCurrent;
                HomePageAttributes.CarbohydratesCurrent = Math.Round(it.Carbohydrates, 1);
                HomePageAttributes.ProteinCurrent = Math.Round(it.Protein, 1);
                HomePageAttributes.FatCurrent = Math.Round(it.Fat, 1);
                HomePageAttributes.SugarCurrent = Math.Round(it.Sugar, 1);
                HomePageAttributes.SaturatedFatCurrent = Math.Round(it.SaturatedFat, 1);
                HomePageAttributes.FiberCurrent = Math.Round(it.Fiber,1);
                HomePageAttributes.SaltCurrent = Math.Round(it.Salt, 1);
                HomePageAttributes.CaloriesProgress = HomePageAttributes.CaloriesGoal == 0 ? 
                    0 : (double)HomePageAttributes.CaloriesCurrent / (double)HomePageAttributes.CaloriesGoal;
                HomePageAttributes.CarbohydratesMacro = RegistrationSettings.Macros
                    .Where(x => x.Key == "carbohydrates").First().Value;
                HomePageAttributes.ProteinMacro = RegistrationSettings.Macros
                    .Where(x => x.Key == "protein").First().Value;
                HomePageAttributes.FatMacro = RegistrationSettings.Macros
                    .Where(x => x.Key == "fat").First().Value;
                HomePageAttributes.CarbohydratesGoal = 
                    Math.Round(HomePageAttributes.CaloriesGoal * (HomePageAttributes.CarbohydratesMacro / 100) / carbohydratesGramKcal, 1);
                HomePageAttributes.ProteinGoal = 
                    Math.Round(HomePageAttributes.CaloriesGoal * (HomePageAttributes.ProteinMacro / 100) / proteinGramKcal, 1);
                HomePageAttributes.FatGoal = 
                    Math.Round(HomePageAttributes.CaloriesGoal * (HomePageAttributes.FatMacro / 100) / fatGramKcal, 1);
                return true;
            }
            return false;
        }


        #region Helpers

        private bool CheckRegistrationSettings()
        {
            bool v1 = RegistrationSettings.ActivityDB > 1; //activity always > 1
            //double,int default value = 0
            bool v2 = RegistrationSettings.AgeDB > 0;
            bool v3 = RegistrationSettings.CaloriesGoal > 0;
            bool v4 = RegistrationSettings.DesiredWeightDB > 1;
            bool v5 = RegistrationSettings.DesiredWeightMeasureDB != string.Empty;
            bool v6 = RegistrationSettings.Email != string.Empty;
            bool v7 = RegistrationSettings.GenderDB != string.Empty;
            bool v8 = RegistrationSettings.GoalDB > 0;
            bool v9 = RegistrationSettings.HeightDB > 0;
            bool v10 = RegistrationSettings.WeightDB > 0;
            bool v11 = RegistrationSettings.WeightMeasureDB != string.Empty;

            return v1 && v2 && v3 && v4 && v5 && v6 && v7 && v8 && v9 && v10 && v11;
        }

        #endregion

        #region Properties 

        private RegistrationSettings _registrationSettings;
        public RegistrationSettings RegistrationSettings
        {
            get { return _registrationSettings; }
            set { SetProperty(ref _registrationSettings, value); }
        }

        private HomePageAttributes _homePageAttributes;
        public HomePageAttributes HomePageAttributes
        {
            get { return _homePageAttributes; }
            set { SetProperty(ref _homePageAttributes, value); }
        }

        #endregion

        #region Commands

        public ICommand RefreshViewCommand { get; set; }

        public ICommand InitializeViewModel { get; set; }

        #endregion
    }
}
