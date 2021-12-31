using FitnessApp01.Helpers;
using FitnessApp01.Interfaces;
using FitnessApp01.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FitnessApp01.ViewModels
{
    public class DiaryPageViewModel : BaseViewModel
    {
        public DiaryPageViewModel()
        {
            //RefreshViewCommand = new Command(execute: async () => await Refresh());
            NextDayCommand = new Command(execute: async () => await NextDay());
            PreviousDayCommand = new Command(execute: async () => await PreviousDay());
            AddMealCommand = new Command<MealGroup>(execute: async (mealgroup) => await AddMeal(mealgroup));
            ItemTapCommand = new Command<Meal>(execute: async (meal) => await ItemTap(meal));
            InitializeViewModel = new Command(execute: async () => await InitializeDiaryPageViewModel());

            //FirestoreBase = DependencyService.Get<IDatabase>();
            //FirestoreBase = Services.FirestoreBase.Instance;
            DiaryPageAttributes = new DiaryPageAttributes();
            MessagingCenter.Subscribe<object>(this, "mealAdded", (p) =>
            {
                OnMessageReceived();
            });
            MessagingCenter.Subscribe<object>(this, "mealEdited", (p) =>
            {
                OnMessageReceived();
            });
            //InitializeDiaryPageViewModel().ContinueWith(OnInitializeComplete);


            

        }

        private async Task ItemTap(Meal meal)
        {
            var jsonString = JsonConvert.SerializeObject(meal);
            await Shell.Current.GoToAsync($"EditMealPage?mealString={jsonString}");
        }

        public async Task AddMeal(MealGroup meals)
        {
            var jsonString = JsonConvert.SerializeObject(RegistrationSettings.Macros);
            await Shell.Current.GoToAsync($"SelectMealPage?mealType={meals.Name}" +
                $"&caloriesGoal={RegistrationSettings.CaloriesGoal}&macros={jsonString}");

        }

        private void OnInitializeComplete(Task arg)
        {
        }

        /*
        private async Task Refresh()
        {
            DiaryPageAttributes.IsRefreshing = false;
            IsBusy = true;
            await LoadAndSetDiaryData();
            IsBusy = false;
        }
        */

        public async Task NextDay()
        {
            DiaryPageAttributes.NameOfTheDay = SelectedDay.Next().ToString("D", Thread.CurrentThread.CurrentCulture);
            await InitializeDiaryPageViewModel();
        }

        public async Task PreviousDay()
        {
            DiaryPageAttributes.NameOfTheDay = SelectedDay.Previous().ToString("D", Thread.CurrentThread.CurrentCulture);
            await InitializeDiaryPageViewModel();
        }

        private void OnMessageReceived()
        {
            var selectedDay = Diary.Days.FirstOrDefault(x => x.UnixSeconds == SelectedDay.ToUnixSecondsString());
            SetDiaryData(selectedDay);
        }

        public async Task InitializeDiaryPageViewModel()
        {
            IsBusy = true;
            bool isValid;
            //nestabilni chovani po vyplneni nedokoncene registrace
            if (RegistrationSettings == null)
            {
                isValid = await LoadRegistrationSettings();
                if (!isValid)
                {
                    await App.Current.MainPage
                        .DisplayAlert("Error", "nedokončená registrace - prosím znovu vyplňtě údaje", "ok");
                    await Shell.Current.GoToAsync("//RegistrationSettingsPage");
                    IsBusy = false;
                }
            }
            
            isValid = await LoadAndSetDiaryData();
            /*if (!isValid)
            {
                await App.Current.MainPage
                    .DisplayAlert("Error", "spatne nactene data", "ok");
            }*/
            IsBusy = false;
        }

        

        public async Task<bool> LoadAndSetDiaryData()
        {
            Day selectedDay;
            try
            {
                selectedDay = Diary.Days.FirstOrDefault(x => x.UnixSeconds == SelectedDay.ToUnixSecondsString());
                //Před komunikací s DB kontrola, jestli se den už nenachází v kolekci
                if (selectedDay == null)
                {
                    //Pokud se den v db nenajde, vrátí se nový "prázdný" den s nastaveným UnixSeconds
                    selectedDay = await FirestoreBase.ReadDiaryDataAsync();
                    Diary.Days.Add(selectedDay);
                }
                return SetDiaryData(selectedDay);
            }
            catch (Exception e)
            {
                await DisplayAlertAsync("Error", "Nepodařilo se stáhnout diář", "ok");
                Console.WriteLine(e.Message);
                //Diary.Days = new ObservableCollection<Day>();
                return false;
            }
        }

        private bool SetDiaryData(Day selectedDay)
        {
            try
            {
                selectedDay.CaloriesGoal = RegistrationSettings.CaloriesGoal;
                DiaryPageAttributes.CaloriesGoal = selectedDay.CaloriesGoal;
                DiaryPageAttributes.CaloriesCurrent = selectedDay.CaloriesCurrent;
                if (DiaryPageAttributes.CaloriesGoal != 0)
                {
                    DiaryPageAttributes.CaloriesProgress = (double)DiaryPageAttributes.CaloriesCurrent / (double)DiaryPageAttributes.CaloriesGoal;
                }
                MealGroups = selectedDay.MealGroups;
                MessagingCenter.Send<object>(this, "diaryUpdated");
                return true;
            }
            catch(Exception e)
            {
                DisplayAlert("Error", "Nepodařilo se nastavit diář. zpráva: " + e.Message, "ok");
                return false;
            }
            
        }

        private async Task<bool> LoadRegistrationSettings()
        {
            try
            {
                RegistrationSettings = await FirestoreBase.ReadRegistrationSettingsAsync();
            }
            catch (Exception)
            {
                RegistrationSettings = new RegistrationSettings();
                await DisplayAlertAsync("Error", "nepodařilo se stáhnout nastavení", "ok");
            }
            return CheckRegistrationSettings();
        }

        #region Helpers

        private bool CheckRegistrationSettings()
        {
            //activity always > 1, range: <1.2,1.725>
            bool v1 = RegistrationSettings.ActivityDB > 1; 
            //double,int default value == 0
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

        //public IDatabase FirestoreBase { get; set; }

        private DiaryPageAttributes _diaryPageAttributes;
        public DiaryPageAttributes DiaryPageAttributes
        {
            get { return _diaryPageAttributes; }
            set 
            {
                //SetProperty(ref _diaryPageAttributes, value);
                if (_diaryPageAttributes != value)
                    _diaryPageAttributes = value;
            }
        }
 
        private ObservableCollection<MealGroup> _mealGroups;
        public ObservableCollection<MealGroup> MealGroups
        {
            get { return _mealGroups; }
            set 
            { 
                SetProperty(ref _mealGroups, value);
            }
        }

        private RegistrationSettings _registrationSettings;
        public RegistrationSettings RegistrationSettings
        {
            get { return _registrationSettings; }
            set { SetProperty(ref _registrationSettings, value); }
        }

        #endregion

        #region Commands

        public ICommand NextDayCommand { get; set; }
        public ICommand PreviousDayCommand { get; set; }
        public ICommand RefreshViewCommand { get; set; }
        public ICommand AddMealCommand { get; set; }
        public ICommand ItemTapCommand { get; set; }
        public ICommand InitializeViewModel { get; set; }

        #endregion
    }
}
