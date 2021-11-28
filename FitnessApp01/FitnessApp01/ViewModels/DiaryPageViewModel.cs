using FitnessApp01.Helpers;
using FitnessApp01.Interfaces;
using FitnessApp01.Models;
using FitnessApp01.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
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
            RefreshViewCommand = new Command(execute: async () => await Refresh());
            NextDayCommand = new Command(execute: async () => await NextDay());
            PreviousDayCommand = new Command(execute: async () => await PreviousDay());
            AddMealCommand = new Command<MealGroup>(execute: async (mealgroup) => await AddMeal(mealgroup));
            ItemTapCommand = new Command<Meal>(execute: async (meal) => await ItemTap(meal));
            FirestoreBase = DependencyService.Get<IDatabase>();
            MessagingCenter.Subscribe<object>(this, "mealAdded", (p) =>
            {
                OnMessageReceived();
            });
            MessagingCenter.Subscribe<object>(this, "mealEdited", (p) =>
            {
                OnMessageReceived();
            });
            InitializeDiaryPageViewModel().ContinueWith(OnInitializeComplete);
            
        }

        private async Task ItemTap(Meal meal)
        {
            var jsonString = JsonConvert.SerializeObject(meal);
            await Shell.Current.GoToAsync($"EditMealPage?mealString={jsonString}");
        }

        private async Task AddMeal(MealGroup meals)
        {
            var jsonString = JsonConvert.SerializeObject(RegistrationSettings.Macros);
            await Shell.Current.GoToAsync($"SelectMealPage?mealType={meals.Name}" +
                $"&caloriesGoal={RegistrationSettings.CaloriesGoal}&macros={jsonString}");

        }

        private void OnInitializeComplete(Task arg)
        {
            
        }

        private async Task Refresh()
        {
            IsRefreshing = false;
            IsRunning = true;
            await LoadDiaryData();
            IsRunning = false;
        }

        private async Task NextDay()
        {
            CurrentDay = SelectedDay.Next();
            await Refresh();
        }

        private async Task PreviousDay()
        {
            CurrentDay = SelectedDay.Previous();
            await Refresh();
        }

        private void OnMessageReceived()
        {
            SetDiaryData(Diary.Days);
        }

        private async Task InitializeDiaryPageViewModel()
        {
            IsRunning = true;
            CurrentDay = SelectedDay.Day;
            //nestabilni chovani po vyplneni nedokoncene registrace
            var isValid = await LoadRegistrationSettings();
            if (!isValid)
            {
                await App.Current.MainPage
                    .DisplayAlert("Error", "nedokončená registrace - prosím znovu vyplňtě údaje", "ok");
                await Shell.Current.GoToAsync("//RegistrationSettingsPage");
                IsRunning = false;
            }
            
            isValid = await LoadDiaryData();
            /*if (!isValid)
            {
                await App.Current.MainPage
                    .DisplayAlert("Error", "spatne nactene data", "ok");
            }*/
            IsRunning = false;
        }

        

        private async Task<bool> LoadDiaryData()
        {
            try
            {
                Diary.Days = await FirestoreBase.ReadDiaryDataAsync();
            }
            catch (Exception)
            {
                await DisplayAlertAsync("Error", "Nepodařilo se stáhnout diář", "ok");
                Diary.Days = new ObservableCollection<Day>();
            }
            return SetDiaryData(Diary.Days);
        }

        private bool SetDiaryData(ObservableCollection<Day> days)
        {
            Day day;
            try
            {
                day = days.First(x => x.UnixSeconds == SelectedDay.ToUnixSecondsString());
            }
            catch (InvalidOperationException)
            {
                day = days.First();
                day.CaloriesGoal = RegistrationSettings.CaloriesGoal;
            }
            CaloriesGoal = day.CaloriesGoal;
            CaloriesCurrent = day.CaloriesCurrent;
            if (CaloriesGoal != 0)
            {
                CaloriesProgress = (double)CaloriesCurrent / (double)CaloriesGoal;
            }
            MealGroups = day.MealGroups;
            MessagingCenter.Send<object>(this, "diaryUpdated");
            return true;
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

        private bool _isRunning = false;
        public bool IsRunning
        {
            get { return _isRunning; }
            set { SetProperty(ref _isRunning, value); }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        private string _nameOfTheDay;
        public string NameOfTheDay
        {
            get { return _nameOfTheDay; }
            set { SetProperty(ref _nameOfTheDay, value); }
        }

        private DateTime _currentDay;
        public DateTime CurrentDay
        {
            get { return _currentDay; }
            set
            {
                SetProperty(ref _currentDay, value);
                NameOfTheDay = value.ToString("D", Thread.CurrentThread.CurrentCulture);
            }
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

        #endregion
    }
}
