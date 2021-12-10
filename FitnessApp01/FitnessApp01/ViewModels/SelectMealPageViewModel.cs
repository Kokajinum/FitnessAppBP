using FitnessApp01.Models;
using FitnessApp01.Services;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FitnessApp01.ViewModels
{
    [QueryProperty(nameof(MealType), "mealType")]
    [QueryProperty(nameof(CaloriesGoal), "caloriesGoal")]
    [QueryProperty(nameof(MacrosString), "macros")]
    public class SelectMealPageViewModel : BaseViewModel
    {
        #region API ALGOLIO
        private readonly string api = "26a4337998d90c8e47df8b2b331fff49";
        private readonly string index = "food";
        #endregion

        public SelectMealPageViewModel()
        {
            SearchService = new SearchService(api, index);
            AddFoodCommand = new Command(async () => await AddFood());
            FoodSearchCommand = new Command<string>(async (searchString) => await FoodSearch(searchString));
            FoodTapCommand = new Command<Food>(async (food) => await SelectMeal(food));
        }

        private async Task SelectMeal(Food food)
        {
            var jsonString = JsonConvert.SerializeObject(food);
            /*
             * food.Name nesmí obsahovat '&', '#', Json pak vyvolá výjimku
             * a) povolit uživateli vložit '&' a poté do databáze uložit 'a'
             * b) znemožnit uživateli vložit '&'
             */

            try
            {
                if (MealType != string.Empty)
                    await Shell.Current.GoToAsync($"AddMealPage?mealType={MealType}&foodJson={jsonString}" +
                        $"&caloriesGoal={CaloriesGoal}&macros={MacrosString}");
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Chyba", "Něco se pokazilo", "ok");
                await Shell.Current.GoToAsync("..");
            }
            
        }

        private async Task FoodSearch(string searchString)
        {
            IsBusy = true;
            var foodList = await SearchService.GetResultsAsync(searchString);
            FoodCollection = new ObservableCollection<Food>(foodList);
            IsBusy = false;
        }

        private async Task AddFood()
        {
            await Shell.Current.GoToAsync("AddFoodPage");
        }

        #region Properties

        private ObservableCollection<Food> _foodCollection;
        public ObservableCollection<Food> FoodCollection
        {
            get { return _foodCollection; }
            set { SetProperty(ref _foodCollection, value); }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                SetProperty(ref _searchText, value);
                if (_searchText.Length == 4 || _searchText.Length == 7)
#pragma warning disable CS4014 // Protože se toto volání neočekává, vykonávání aktuální metody pokračuje před dokončením volání.
                    FoodSearch(_searchText);
#pragma warning restore CS4014 // Protože se toto volání neočekává, vykonávání aktuální metody pokračuje před dokončením volání.
            }
        }

        public SearchService SearchService { get; set; }

        public string MealType { get; set; }

        public int CaloriesGoal { get; set; }

        public string MacrosString { get; 
            set; }

        #endregion

        #region Commands

        public ICommand AddFoodCommand { get; set; }
        public ICommand AddMealCommand { get; set; }
        public ICommand FoodSearchCommand { get; set; }
        public ICommand FoodTapCommand { get; set; }

        #endregion
    }
}
