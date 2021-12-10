using FitnessApp01.Interfaces;
using FitnessApp01.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FitnessApp01.ViewModels
{
    [QueryProperty(nameof(MealType), "mealType")]
    [QueryProperty(nameof(FoodJson), "foodJson")]
    [QueryProperty(nameof(CaloriesGoal), "caloriesGoal")]
    [QueryProperty(nameof(MacrosString), "macros")]
    public class AddMealPageViewModel : BaseViewModel
    {
        public AddMealPageViewModel()
        {
            AddMealCommand = new Command(
                execute: async () => await AddMeal(),
                canExecute: () => AddCanExecute());
            FirestoreBase = DependencyService.Get<IDatabase>();
            //InitializeAddMealPageViewModel();
        }

        private bool AddCanExecute()
        {
            return CanAdd;
        }

        private async Task AddMeal()
        {
            IsRunning = true;
            var meal = new Meal(Food.Name, Food.Brand, WeightCalculated, KcalCalculated, CarbsCalculated, SugarCalculated,
                    ProteinCalculated, FatCalculated, SaturatedCalculated, FiberCalculated, SaltCalculated, MealType,
                    Food.Kcal, Food.Carbohydrates, Food.Sugar, Food.Protein, Food.Fat, Food.SaturatedFat, Food.Fiber,
                    Food.Salt);
            if (string.IsNullOrEmpty(MealType))
                new Exception("MealType is null");
            try
            {
                //pokud jeste neexistuje den tak se vyvola vyjimka
                var day = Diary.Days.First(x => x.UnixSeconds == SelectedDay.ToUnixSecondsString());
                var oldDay = day.Clone();
                var mealGroup = day.MealGroups.FirstOrDefault(x => x.Name == MealType);
                mealGroup.Add(meal);
                //updatnou vsechny dokumenty + pridat nove
                var updatedDay = UpdateExistingDay(day);
                try
                {
                    await FirestoreBase.CreateDayAsync(updatedDay);
                    await FirestoreBase.CreateMealAsync(meal);
                }
                catch (Exception)
                {
                    mealGroup.Remove(meal);
                    CleanUp(oldDay);
                }
            } 
            //inicializace noveho dne
            catch (InvalidOperationException)
            {
                var oldDay = Diary.Days.FirstOrDefault().Clone();
                var newDay = InitializeNewDay(Diary.Days.FirstOrDefault());
                var mealGroup = newDay.MealGroups.FirstOrDefault(x => x.Name == MealType);
                mealGroup.Add(meal);
                try
                {
                    await FirestoreBase.CreateDayAsync(newDay);
                    await FirestoreBase.CreateMealAsync(meal);
                }
                catch (Exception)
                {
                    mealGroup.Remove(meal);
                    CleanUp(oldDay);
                }
            }
            finally
            {
                await GoToPageAsync("//main-content");
                IsRunning = false;
                MessagingCenter.Send<object>(this, "mealAdded");
            }
            
        }

        //bude implementovat model Day
        private void CleanUp(Day oldDay)
        {
            int i;
            for (i = 0; i < Diary.Days.Count; i++)
            {
                if (Diary.Days[i].UnixSeconds == SelectedDay.ToUnixSecondsString())
                {
                    Diary.Days[i] = oldDay;
                }
            }
        }


        //nedava smysl, day je predavan referenci, menen, a pak i vracen --> opravit
        // bude implementovat model Day
        private Day InitializeNewDay(Day day)
        {
            day.UnixSeconds = SelectedDay.ToUnixSecondsString();
            day.CaloriesGoal = CaloriesGoal;
            day.CaloriesCurrent = KcalCalculated;
            day.Protein = ProteinCalculated;
            day.Carbohydrates = CarbsCalculated;
            day.Fat = FatCalculated;
            day.SaturatedFat = SaturatedCalculated;
            day.Fiber = FiberCalculated;
            day.Salt = SaltCalculated;
            day.Sugar = SugarCalculated;
            day.Macros = Macros;
            return day;
        }

        private Day UpdateExistingDay(Day day)
        {
            day.CaloriesCurrent += KcalCalculated;
            day.Protein += ProteinCalculated;
            day.Carbohydrates += CarbsCalculated;
            day.Fat += FatCalculated;
            day.SaturatedFat += SaturatedCalculated;
            day.Fiber += FiberCalculated;
            day.Salt += SaltCalculated;
            day.Sugar += SugarCalculated;
            return day;
        }

        private void InitializeAddMealPageViewModel()
        {
            PickerSource = new ObservableCollection<string>();
            if (string.IsNullOrEmpty(Food.Measure))
            {
                Food.Measure = "g";
            }
            if (Food.PortionSize != 0)
            {
                PickerSource.Add(" x " + Food.PortionSize.ToString() + Food.Measure);
                PickerSource.Add(Food.Measure);
            }
            else
            {
                PickerSource.Add(Food.Measure);
            }
            PickerCurrentItem = PickerSource[0];
        }

        private Food ConvertJsonToFood(string json)
        {
            return JsonConvert.DeserializeObject<Food>(json);
        }

        private void CalculateNutrients()
        {
            WeightCalculated = (double)UserInput;
            if (PickerCurrentItem != Food.Measure)
            {
                WeightCalculated *= Food.PortionSize;
            }
            KcalCalculated = (int)(Food.Kcal * WeightCalculated / 100);
            CarbsCalculated = (double)(Food.Carbohydrates * WeightCalculated / 100);
            FatCalculated = (double)(Food.Fat * WeightCalculated / 100);
            ProteinCalculated = (double)(Food.Protein * WeightCalculated / 100);
            SugarCalculated = (double)(Food.Sugar * WeightCalculated / 100);
            SaturatedCalculated = (double)(Food.SaturatedFat * WeightCalculated / 100);
            FiberCalculated = (double)(Food.Fiber * WeightCalculated / 100);
            SaltCalculated = (double)(Food.Salt * WeightCalculated / 100);
        }

        #region Properties 

        private IDatabase FirestoreBase { get; 
            set; }
        public string MealType { get; 
            set; }

        private string _foodJson;
        public string FoodJson
        {
            get { return _foodJson; }
            set 
            { 
                SetProperty(ref _foodJson, value);
                Food = ConvertJsonToFood(value);
                InitializeAddMealPageViewModel();
            }
        }

        public int CaloriesGoal { get; set; }

        private string _macrosString;
        public string MacrosString
        {
            get { return _macrosString; }
            set
            {
                SetProperty(ref _macrosString, value);
                Macros = JsonConvert.DeserializeObject<Dictionary<string, double>>(_macrosString);
            }
        }

        public Dictionary<string, double> Macros { get; set; }

        private Food _food;
        public Food Food
        {
            get { return _food; }
            set { SetProperty(ref _food, value); }
        }

        private double _weightCalculated;
        public double WeightCalculated
        {
            get { return _weightCalculated; }
            set { SetProperty(ref _weightCalculated, value); }
        }

        private int _kcalCalculated;
        public int KcalCalculated
        {
            get { return _kcalCalculated; }
            set { SetProperty(ref _kcalCalculated, value); }
        }

        private double _carbsCalculated;
        public double CarbsCalculated
        {
            get { return _carbsCalculated; }
            set { SetProperty(ref _carbsCalculated, value); }
        }

        private double _fatCalculated;
        public double FatCalculated
        {
            get { return _fatCalculated; }
            set { SetProperty(ref _fatCalculated, value); }
        }

        private double _proteinCalculated;
        public double ProteinCalculated
        {
            get { return _proteinCalculated; }
            set { SetProperty(ref _proteinCalculated, value); }
        }

        private double _sugarCalculated;
        public double SugarCalculated
        {
            get { return _sugarCalculated; }
            set { SetProperty(ref _sugarCalculated, value); }
        }

        private double _saturatedCalculated;
        public double SaturatedCalculated
        {
            get { return _saturatedCalculated; }
            set { SetProperty(ref _saturatedCalculated, value); }
        }

        private double _fiberCalculated;
        public double FiberCalculated
        {
            get { return _fiberCalculated; }
            set { SetProperty(ref _fiberCalculated, value); }
        }

        private double _saltCalculated;
        public double SaltCalculated
        {
            get { return _saltCalculated; }
            set { SetProperty(ref _saltCalculated, value); }
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                SetProperty(ref _isVisible, value);
            }
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get { return _isRunning; }
            set { SetProperty(ref _isRunning, value); }
        }

        private double? _userInput;
        public double? UserInput
        {
            get { return _userInput; }
            set 
            { 
                SetProperty(ref _userInput, value);
                if (_userInput == null)
                {
                    IsVisible = false;
                }else
                {
                    CalculateNutrients();
                    IsVisible = true;
                }
                OnPropertyChanged("CanAdd");

            }
        }

        private ObservableCollection<string> _pickerSource;
        public ObservableCollection<string> PickerSource
        {
            get { return _pickerSource; }
            set { SetProperty(ref _pickerSource, value); }
        }

        private string _pickerCurrentItem;
        public string PickerCurrentItem
        {
            get { return _pickerCurrentItem; } 
            set 
            { 
                SetProperty(ref _pickerCurrentItem, value);
                if (_userInput != null)
                {
                    CalculateNutrients();
                }
            }
        }

        public bool CanAdd
        {
            get
            {
                return UserInput != null;
            }
        }

        #endregion

        #region Commands

        public ICommand AddMealCommand { get; set; }

        #endregion
    }
}
