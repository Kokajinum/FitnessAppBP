using FitnessApp01.Helpers;
using FitnessApp01.Models;
using FitnessApp01.Resx;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FitnessApp01.Services
{
    public class FirestoreBase
    {
        public static async Task InsertRegistrationSettings(RegistrationSettings registrationSettings)
        {
            try
            {
                await CrossCloudFirestore.Current.Instance
                    .Collection("users")
                    .Document(AuthBase.GetUserId())
                    .SetAsync(registrationSettings);
            }
            catch (CloudFirestoreException e)
            {
                throw new Exception(e.Message + " " + e.ErrorType, e.InnerException);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public static async Task<RegistrationSettings> LoadRegistrationSettings()
        {
            try
            {
                var doc = await CrossCloudFirestore.Current.Instance
                    .Document("users/" + AuthBase.GetUserId())
                    .GetAsync();
                return doc.Exists ? doc.ToObject<RegistrationSettings>() : new RegistrationSettings();
            }
            catch (CloudFirestoreException e)
            {
                throw new Exception(e.Message + " " + e.ErrorType, e.InnerException);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public static async Task<List<Meal>> LoadMealData(string meal)
        {
            try
            {
                var group = await CrossCloudFirestore.Current.Instance
                    //.Collection("/diary/6tyEdoVGxI10YXSdxJdT/days/1627171200/" + meal)
                    .Collection("/diary/" + AuthBase.GetUserId() + 
                    "/days/" + SelectedDay.ToUnixSecondsString() + "/" + meal)
                    .GetAsync();
                if (!group.IsEmpty)
                {
                    var models = group.ToObjects<Meal>();
                    return new List<Meal>(models);
                }
                return new List<Meal>();
            }
            catch (CloudFirestoreException e)
            {
                throw new Exception(e.Message + " " + e.ErrorType, e.InnerException);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public static async Task<ObservableCollection<Day>> LoadDiaryData()
        {
            try
            {
                var obs = new ObservableCollection<Day>();
                Console.WriteLine(AuthBase.GetUserId());
                Console.WriteLine(SelectedDay.ToUnixSecondsString());
                Console.WriteLine("/diary/" + AuthBase.GetUserId() + "/days/" + SelectedDay.ToUnixSecondsString());
                var doc = await CrossCloudFirestore.Current.Instance
                    .Document("/diary/" + AuthBase.GetUserId() + "/days/" + SelectedDay.ToUnixSecondsString())
                    .GetAsync();
                if (doc.Exists)
                {
                    var model = doc.ToObject<Day>();
                    //
                    var breakfastListMeals = await LoadMealData("breakfast");
                    model.MealGroups.Add(new MealGroup("breakfast", AppResources.Breakfast, breakfastListMeals));
                    //
                    var lunchListMeals = await LoadMealData("lunch");
                    model.MealGroups.Add(new MealGroup("lunch", AppResources.Lunch, lunchListMeals));
                    //
                    var snackListMeals = await LoadMealData("snack");
                    model.MealGroups.Add(new MealGroup("snack", AppResources.Snack, snackListMeals));
                    //
                    var dinnerListMeals = await LoadMealData("dinner");
                    model.MealGroups.Add(new MealGroup("dinner", AppResources.Dinner, dinnerListMeals));
                    //
                    obs.Add(model);
                    return obs;
                }
                else
                {
                    var model = CreateEmptyDay();
                    obs.Add(model);
                    return obs;
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.Message, "ok");
                var model = CreateEmptyDay();
                var obs = new ObservableCollection<Day>();
                obs.Add(model);
                return obs;
            }
        }

        private static Day CreateEmptyDay()
        {
            var day = new Day();
            day.MealGroups.Add(new MealGroup("breakfast", AppResources.Breakfast, new List<Meal>()));
            day.MealGroups.Add(new MealGroup("lunch", AppResources.Lunch, new List<Meal>()));
            day.MealGroups.Add(new MealGroup("snack", AppResources.Snack, new List<Meal>()));
            day.MealGroups.Add(new MealGroup("dinner", AppResources.Dinner, new List<Meal>()));
            return day;
        }

        public static async Task SaveFoodData(Food food)
        {
            try
            {
                await CrossCloudFirestore.Current.Instance
                    .Collection("food")
                    .AddAsync(food);
            }
            catch (CloudFirestoreException e)
            {
                throw new Exception(e.Message + " " + e.ErrorType, e.InnerException);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        /*public static async Task<IDocumentReference> GetFoodReference(string name)
        {
            IDocumentReference foodRef = null;
            try
            {
                var query = await CrossCloudFirestore.Current.Instance
                    .Collection("food").WhereEqualsTo("name", name).LimitTo(1).GetAsync();
                if (!query.IsEmpty)
                {
                    foodRef = query.Documents.First().Reference;
                    return foodRef;
                }
                return foodRef;
            }
            catch (CloudFirestoreException e)
            {
                throw new Exception(e.Message + " " + e.ErrorType, e.InnerException);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public static async Task<Food> GetFoodAsync(string id)
        {
            try
            {
                var doc = await CrossCloudFirestore.Current.Instance
                    .Document("/food/" + id)
                    .GetAsync();
                return doc.Exists ? doc.ToObject<Food>() : new Food();
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.Message, "ok");
                await Shell.Current.GoToAsync("..");
                return new Food();
            }
        }*/

        public static async Task InsertNewDayAsync(Day newDay)
        {
            try
            {
                await CrossCloudFirestore.Current.Instance
                    .Document("/diary/" + AuthBase.GetUserId() + "/days/" + SelectedDay.ToUnixSecondsString())
                    .SetAsync(newDay);
            }
            catch (CloudFirestoreException e)
            {
                throw new Exception(e.Message + " " + e.ErrorType, e.InnerException);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public static async Task InsertNewMealAsync(Meal newMeal)
        {
            try
            {
                await CrossCloudFirestore.Current.Instance
                    .Collection("/diary/" + AuthBase.GetUserId() + "/days/" + SelectedDay.ToUnixSecondsString()
                    + "/" + newMeal.MealType + "/")
                    .AddAsync(newMeal);
            }
            catch (CloudFirestoreException e)
            {
                throw new Exception(e.Message + " " + e.ErrorType, e.InnerException);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public static async Task UpdateMealAsync(Meal updatedMeal)
        {
            try
            {
                var q = await CrossCloudFirestore.Current.Instance
                    .Collection("/diary/" + AuthBase.GetUserId() + "/days/" + SelectedDay.ToUnixSecondsString() + "/" + updatedMeal.MealType)
                    .WhereEqualsTo("name", updatedMeal.Name)
                    .LimitTo(1)
                    .GetAsync();
                if (q.IsEmpty)
                    throw new CloudFirestoreException("err", ErrorType.NotFound);
                await q.Documents.First().Reference.UpdateAsync(updatedMeal);
            }
            catch (CloudFirestoreException e)
            {
                throw new Exception(e.Message + " " + e.ErrorType, e.InnerException);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public static async Task UpdateDayAsync(Day updatedDay)
        {
            try
            {
                await CrossCloudFirestore.Current.Instance
                    .Document("/diary/" + AuthBase.GetUserId() + "/days/" + SelectedDay.ToUnixSecondsString())
                    .UpdateAsync(updatedDay);
            }
            catch (CloudFirestoreException e)
            {
                throw new Exception(e.Message + " " + e.ErrorType, e.InnerException);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public static async Task RemoveMealAsync(Meal meal)
        {
            try
            {
                var q = await CrossCloudFirestore.Current.Instance
                    .Collection("/diary/" + AuthBase.GetUserId() + "/days/" + SelectedDay.ToUnixSecondsString() + "/" + meal.MealType)
                    .WhereEqualsTo("name", meal.Name)
                    .LimitTo(1)
                    .GetAsync();
                if (q.IsEmpty)
                    throw new CloudFirestoreException("err", ErrorType.NotFound);
                await q.Documents.First().Reference.DeleteAsync();
            }
            catch (CloudFirestoreException e)
            {
                throw new Exception(e.Message + " " + e.ErrorType, e.InnerException);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }
    }
}
