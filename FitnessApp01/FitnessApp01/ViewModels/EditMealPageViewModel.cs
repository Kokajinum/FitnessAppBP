using FitnessApp01.Models;
using FitnessApp01.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FitnessApp01.ViewModels
{
    [QueryProperty(nameof(MealString), "mealString")]
    public class EditMealPageViewModel : BaseViewModel
    {
        public EditMealPageViewModel()
        {
            EditMealCommand = new Command(
                execute: async () => await EditMeal(),
                canExecute: () => EditCanExecute());
            DeleteMealCommand = new Command(
                execute: async () => await DeleteMeal());
        }

        private async Task DeleteMeal()
        {
            IsRunning = true;
            try
            {
                RemoveMealFromMealGroup();
                WeightInput = 0;
                CalculateNutrients();
                UpdateCurrentDay();
                await FirestoreBase.UpdateDayAsync(CurrentDay);
                await FirestoreBase.RemoveMealAsync(Meal);
            }
            //todo reseni chyb
            catch (Exception e)
            {

            }
            finally
            {
                IsRunning = false;
                await Shell.Current.GoToAsync("..");
                MessagingCenter.Send<object>(this, "mealEdited");
            }
            
        }

        private async Task EditMeal()
        {
            IsRunning = true;
            try
            {
                var mealgroup = RemoveMealFromMealGroup(); 
                UpdateCurrentDay();
                await FirestoreBase.UpdateDayAsync(CurrentDay);
                UpdateMeal();
                await FirestoreBase.UpdateMealAsync(Meal);
                mealgroup.Add(Meal);
                
               
            }
            //todo
            catch (Exception e)
            {
                
            }
            finally
            {
                IsRunning = false;
                await Shell.Current.GoToAsync("..");
                MessagingCenter.Send<object>(this, "mealEdited");
            }
            
        }

        private void InitializeEditMealPageViewModel()
        {
            MealName = Meal.Name;
            MealBrand = Meal.Brand;
            WeightInput = WeightUnchanged = Meal.Weight;
            KcalCalculated = Meal.Kcal;
            CarbsCalculated = Meal.Carbohydrates;
            ProteinCalculated = Meal.Protein;
            FatCalculated = Meal.Fat;
        }



        #region helpers

        private bool EditCanExecute()
        {
            return CanEdit;
        }

        private void UpdateCurrentDay()
        {
            //kcal = aktualni hodnota - nova vypocitana hodnota
            //kcal > 0 znamená snížení váhy
            //kcal < 0         zvýšení váhy
            var kcal = Meal.Kcal - KcalCalculated;
            var carbohydrates = Meal.Carbohydrates - CarbsCalculated;
            var protein = Meal.Protein - ProteinCalculated;
            var fat = Meal.Fat - FatCalculated;
            var fiber = Meal.Fiber - FiberCalculated;
            var salt = Meal.Salt - SaltCalculated;
            var saturatedFat = Meal.SaturatedFat - SaturatedCalculated;
            var sugar = Meal.Sugar - SugarCalculated;

            CurrentDay.CaloriesCurrent -= kcal;
            CurrentDay.Carbohydrates -= carbohydrates;
            CurrentDay.Protein -= protein;
            CurrentDay.Fat -= fat;
            CurrentDay.Fiber -= fiber;
            CurrentDay.Salt -= salt;
            CurrentDay.SaturatedFat -= saturatedFat;
            CurrentDay.Sugar -= sugar;
        }

        private MealGroup RemoveMealFromMealGroup()
        {
            var mealgroup = GetMealGroup();
            var mealToRemove = mealgroup.First(x => x.Name == Meal.Name && x.Weight == Meal.Weight);
            mealgroup.Remove(mealToRemove);
            return mealgroup;
        }

        private MealGroup GetMealGroup()
        {
            CurrentDay = Diary.Days.First(x => x.UnixSeconds == SelectedDay.ToUnixSecondsString());
            var mealgroup = CurrentDay.MealGroups.First(x => x.Name == Meal.MealType);
            return mealgroup;
        }

        private void UpdateMeal()
        {
            Meal.Weight = (double)WeightInput;
            Meal.Kcal = KcalCalculated;
            Meal.Carbohydrates = CarbsCalculated;
            Meal.Protein = ProteinCalculated;
            Meal.Fat = FatCalculated;
            Meal.Sugar = SugarCalculated;
            Meal.SaturatedFat = SaturatedCalculated;
            Meal.Fiber = FiberCalculated;
            Meal.Salt = SaltCalculated;
        }

        private void CalculateNutrients()
        {
            KcalCalculated = (int)(Meal.KcalOrig * WeightInput / 100);
            CarbsCalculated = (double)(Meal.CarbohydratesOrig * WeightInput / 100);
            FatCalculated = (double)(Meal.FatOrig * WeightInput / 100);
            ProteinCalculated = (double)(Meal.ProteinOrig * WeightInput / 100);
            SugarCalculated = (double)(Meal.SugarOrig * WeightInput / 100);
            SaturatedCalculated = (double)(Meal.SaturatedFatOrig * WeightInput / 100);
            FiberCalculated = (double)(Meal.FiberOrig * WeightInput / 100);
            SaltCalculated = (double)(Meal.SaltOrig * WeightInput / 100);
        }

        #endregion

        #region Properties

        public Food Food { get; set; }

        public Day CurrentDay { get; set; }

        private Meal _meal;
        public Meal Meal
        {
            get { return _meal; }
            set { SetProperty(ref _meal, value); }
        }

        private string _mealString;
        public string MealString
        {
            get { return _mealString; }
            set
            { 
                SetProperty(ref _mealString, value);
                Meal = JsonConvert.DeserializeObject<Meal>(_mealString);
                InitializeEditMealPageViewModel();
            }
        }

        private string _mealName;
        public string MealName
        {
            get { return _mealName; }
            set { SetProperty(ref _mealName, value); }
        }

        private string _mealBrand;
        public string MealBrand
        {
            get { return _mealBrand; }
            set { SetProperty(ref _mealBrand, value); }
        }

        private double? _weightInput;
        public double? WeightInput
        {
            get { return _weightInput; }
            set
            { 
                SetProperty(ref _weightInput, value);
                if (_weightInput == null)
                {
                    IsVisible = false;
                }
                else
                {
                    CalculateNutrients();
                    IsVisible = true;
                }
                OnPropertyChanged("CanEdit");
            }
        }

        public double? WeightUnchanged { get; set; }

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
            set { SetProperty(ref _isVisible, value); }
        }

        public bool CanEdit
        {
            get
            {
                return WeightInput != null && WeightInput != WeightUnchanged;
            }
        }

        private bool _isRunning;
        public bool IsRunning 
        { 
            get { return _isRunning; }
            set { SetProperty(ref _isRunning, value); }
        }

        #endregion

        #region Commands

        public ICommand EditMealCommand { get; set; }
        public ICommand DeleteMealCommand { get; set; }

        #endregion
    }
}
