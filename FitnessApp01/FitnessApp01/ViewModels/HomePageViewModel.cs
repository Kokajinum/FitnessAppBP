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
            //FirestoreBase = DependencyService.Get<IDatabase>();
            FirestoreBase = Services.FirestoreBase.Instance;

            MessagingCenter.Subscribe<object>(this, "diaryUpdated", (p) =>
            {
                OnMessageReceived();
            });
            InitializeHomePageViewModel().ConfigureAwait(true);

        }

        private void Refresh()
        {
            SetDiaryData();
            IsRefreshing = false;
        }

        private void OnMessageReceived()
        {
            IsBusy = true;
            SetDiaryData();
            IsBusy = false;
        }

        //private void PrintSomething(Task arg1)
        //{
           
        //}

        private async Task InitializeHomePageViewModel()
        {
            IsBusy = true;
            var isValid = await LoadRegistrationSettings();
            if (!isValid)
            {
                await App.Current.MainPage
                    .DisplayAlert("Error", "nedokončená registrace - prosím znovu vyplňtě údaje", "ok");
                await Shell.Current.GoToAsync("//RegistrationSettingsPage");
            }

            /*isValid = await LoadDiaryData();
            if (!isValid)
            {
                await App.Current.MainPage
                    .DisplayAlert("Error", "spatne nactene data", "ok");
            }*/
            SetDiaryData();
            IsBusy = false;
        }

        private async Task<bool> LoadRegistrationSettings()
        {
            RegistrationSettings = await FirestoreBase.ReadRegistrationSettingsAsync();
            return CheckRegistrationSettings();
        }

        /*private async Task<bool> LoadDiaryData()
        {
            Diary.Days = await FirestoreBase.LoadDiaryData();
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
                CaloriesGoal = RegistrationSettings.CaloriesGoal;
                CaloriesCurrent = it.CaloriesCurrent;
                CarbohydratesCurrent = Math.Round(it.Carbohydrates, 1);
                ProteinCurrent = Math.Round(it.Protein, 1);
                FatCurrent = Math.Round(it.Fat, 1);
                SugarCurrent = Math.Round(it.Sugar, 1);
                SaturatedFatCurrent = Math.Round(it.SaturatedFat, 1);
                FiberCurrent = Math.Round(it.Fiber,1);
                SaltCurrent = Math.Round(it.Salt, 1);
                CaloriesProgress = CaloriesGoal == 0 ? 0 : (double)CaloriesCurrent / (double)CaloriesGoal;
                CarbohydratesMacro = RegistrationSettings.Macros
                    .Where(x => x.Key == "carbohydrates").First().Value;
                ProteinMacro = RegistrationSettings.Macros
                    .Where(x => x.Key == "protein").First().Value;
                FatMacro = RegistrationSettings.Macros
                    .Where(x => x.Key == "fat").First().Value;
                CarbohydratesGoal = Math.Round(CaloriesGoal * (CarbohydratesMacro / 100) / carbohydratesGramKcal, 1);
                ProteinGoal = Math.Round(CaloriesGoal * (ProteinMacro / 100) / proteinGramKcal, 1);
                FatGoal = Math.Round(CaloriesGoal * (FatMacro / 100) / fatGramKcal, 1);
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

        private IDatabase FirestoreBase { get; set; }

        private RegistrationSettings _registrationSettings;
        public RegistrationSettings RegistrationSettings
        {
            get { return _registrationSettings; }
            set { SetProperty(ref _registrationSettings, value); }
        }

        private int _caloriesGoal;
        public int CaloriesGoal
        {
            get { return _caloriesGoal; }
            set { SetProperty(ref _caloriesGoal, value); }
        }

        private int _caloriesCurrent;
        public int CaloriesCurrent       
        {
            get { return _caloriesCurrent; }
            set { SetProperty(ref _caloriesCurrent, value); }
        }

        private double _caloriesProgress;
        public double CaloriesProgress
        {
            get { return _caloriesProgress; }
            set { SetProperty(ref _caloriesProgress, value); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        private double _carbohydratesCurrent;
        public double CarbohydratesCurrent
        {
            get { return _carbohydratesCurrent; }
            set { SetProperty(ref _carbohydratesCurrent, value); }
        }

        private double _proteinCurrent;
        public double ProteinCurrent
        {
            get { return _proteinCurrent; }
            set { SetProperty(ref _proteinCurrent, value); }
        }

        private double _fatCurrent;
        public double FatCurrent
        {
            get { return _fatCurrent; }
            set { SetProperty(ref _fatCurrent, value); }
        }

        private double _carbohydratesGoal;
        public double CarbohydratesGoal
        {
            get { return _carbohydratesGoal; }
            set { SetProperty(ref _carbohydratesGoal, value); }
        }

        private double _proteinGoal;
        public double ProteinGoal
        {
            get { return _proteinGoal; }
            set { SetProperty(ref _proteinGoal, value); }
        }

        private double _fatGoal;
        public double FatGoal
        {
            get { return _fatGoal; }
            set { SetProperty(ref _fatGoal, value); }
        }

        private double _saturatedFatCurrent;
        public double SaturatedFatCurrent
        {
            get { return _saturatedFatCurrent; }
            set { SetProperty(ref _saturatedFatCurrent, value); }
        }

        private double _sugarCurrent;
        public double SugarCurrent
        {
            get { return _sugarCurrent; }
            set { SetProperty(ref _sugarCurrent, value); }
        }

        private double _fiberCurrent;
        public double FiberCurrent
        {
            get { return _fiberCurrent; }
            set { SetProperty(ref _fiberCurrent, value); }
        }

        private double _saltCurrent;
        public double SaltCurrent
        {
            get { return _saltCurrent; }
            set { SetProperty(ref _saltCurrent, value); }
        }

        private double _carbohydratesMacro;
        public double CarbohydratesMacro
        {
            get { return _carbohydratesMacro; }
            set { SetProperty(ref _carbohydratesMacro, value); }
        }

        private double _proteinMacro;
        public double ProteinMacro
        {
            get { return _proteinMacro; }
            set { SetProperty(ref _proteinMacro, value); }
        }

        private double _fatMacro;
        public double FatMacro
        {
            get { return _fatMacro; }
            set { SetProperty(ref _fatMacro, value); }
        }
        #endregion

        #region Commands

        public ICommand RefreshViewCommand { get; set; }

        #endregion
    }
}
