using FitnessApp01.Helpers;
using FitnessApp01.Interfaces;
using FitnessApp01.Models;
using FitnessApp01.Resx;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;

namespace FitnessApp01.ViewModels
{
    public class AddMealPageViewModel : BaseViewModel, IQueryAttributable
    {
        public AddMealPageViewModel()
        {
            AddMealCommand = new Command(
                execute: async () => await AddMeal());
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            MealType = HttpUtility.UrlDecode(query["mealType"]);
            var caloriesGoalJson = HttpUtility.UrlDecode(query["caloriesGoal"]);
            CaloriesGoal = int.Parse(caloriesGoalJson);
            FoodJson = HttpUtility.UrlDecode(query["foodJson"]);
            MacrosJson = HttpUtility.UrlDecode(query["macros"]);

            Food = JsonToObject<Food>(FoodJson);
            Macros = JsonToObject<Dictionary<string, double>>(MacrosJson);

            InitializeAddMealPageViewModel();
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
                UserInput = 1;
                PickerSource.Add(" x " + Food.PortionSize.ToString() + Food.Measure);
                PickerSource.Add(Food.Measure);
            }
            else
            {
                PickerSource.Add(Food.Measure);
            }
            PickerCurrentItem = PickerSource[0];

        }

        public async Task AddMeal()
        {
            if (!CanAdd)
            {
                await DisplayErrorAsync(AppResources.CanNotSave);
                return;
            }
            IsBusy = true;
            MealGroup mealGroup;
            Day oldDay;
            var meal = new Meal(Food.Name, Food.Brand, WeightCalculated, KcalCalculated, CarbsCalculated, SugarCalculated,
                    ProteinCalculated, FatCalculated, SaturatedCalculated, FiberCalculated, SaltCalculated, MealType,
                    Food.Kcal, Food.Carbohydrates, Food.Sugar, Food.Protein, Food.Fat, Food.SaturatedFat, Food.Fiber,
                    Food.Salt);
            
            try
            {
                if (string.IsNullOrEmpty(MealType))
                    throw new Exception("MealType is null");

                //day == null pokud z nejakeho duvodu neexistuje, ale mělo by vždy již existovat
                var day = Diary.Days.FirstOrDefault(x => x.UnixSeconds == SelectedDay.ToUnixSecondsString());
                if (day != null)
                {
                    oldDay = day.Clone();
                    mealGroup = day.MealGroups.FirstOrDefault(x => x.Name == MealType);
                }
                else
                {
                    day = new Day();
                    Diary.Days.Add(day);
                    oldDay = day.Clone();
                    var newDay = InitializeNewDay(day);
                    mealGroup = newDay.MealGroups.FirstOrDefault(x => x.Name == MealType);
                }
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
            catch (Exception)
            {

            }
            finally
            {
                await GoToPageAsync("//main-content");
                IsBusy = false;
                MessagingCenter.Send<object>(this, "mealAdded");
            }
            
        }

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
            day.MealGroups.Add(new MealGroup("breakfast", AppResources.Breakfast, new List<Meal>()));
            day.MealGroups.Add(new MealGroup("lunch", AppResources.Lunch, new List<Meal>()));
            day.MealGroups.Add(new MealGroup("snack", AppResources.Snack, new List<Meal>()));
            day.MealGroups.Add(new MealGroup("dinner", AppResources.Dinner, new List<Meal>()));
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

        

        private T JsonToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
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
        public string MealType { get; 
            set; }

        private string _foodJson;
        public string FoodJson
        {
            get { return _foodJson; }
            set 
            { 
                SetProperty(ref _foodJson, value);
            }
        }

        public int CaloriesGoal { get; set; }

        private string _macrosString;
        public string MacrosJson
        {
            get { return _macrosString; }
            set
            {
                SetProperty(ref _macrosString, value);
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

        private double? _userInput;
        public double? UserInput
        {
            get { return _userInput; }
            set 
            { 
                SetProperty(ref _userInput, value);
                CanAddChanged();
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
                return UserInput != null && UserInput > 0;
            }
        }

        private double _addButtonOpacity = 0.2;
        public double AddButtonOpacity
        {
            get { return _addButtonOpacity; }
            set => SetProperty(ref _addButtonOpacity, value);
        }

        private void CanAddChanged()
        {
            if (CanAdd)
            {
                CalculateNutrients();
                IsVisible = true;
                AddButtonOpacity = 1;
            }
            else
            {
                IsVisible = false;
                AddButtonOpacity = 0.2;
            }
        }

        #endregion

        #region Commands

        public ICommand AddMealCommand { get; set; }

        #endregion
    }
}
