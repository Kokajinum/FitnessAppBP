using Newtonsoft.Json;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace FitnessApp01.Models
{
    public class MealsOfTheDay
    {
        public Breakfast Breakfast { get; set; }
        public Lunch Lunch { get; set; }
        public Snack Snack { get; set; }
        public Dinner Dinner { get; set; }
    }

    public class Meal
    {
        [MapTo("name")]
        public string Name { get; set; }
        [MapTo("brand")]
        public string Brand { get; set; }
        [MapTo("kcal")]
        public int Kcal { get; set; }
        [MapTo("carbohydrates")]
        public double Carbohydrates { get; set; }
        [MapTo("sugar")]
        public double Sugar { get; set; }
        [MapTo("protein")]
        public double Protein { get; set; }
        [MapTo("fat")]
        public double Fat { get; set; }
        [MapTo("saturatedFat")]
        public double SaturatedFat { get; set; }
        [MapTo("fiber")]
        public double Fiber { get; set; }
        [MapTo("salt")]
        public double Salt { get; set; }
        [MapTo("weight")]
        public double Weight { get; set; }
        [MapTo("mealType")]
        public string MealType { get; set; }
        [MapTo("kcalOrig")]
        public int KcalOrig { get; set; }
        [MapTo("carbohydratesOrig")]
        public double CarbohydratesOrig { get; set; }
        [MapTo("sugarOrig")]
        public double SugarOrig { get; set; }
        [MapTo("proteinOrig")]
        public double ProteinOrig { get; set; }
        [MapTo("fatOrig")]
        public double FatOrig { get; set; }
        [MapTo("saturatedFatOrig")]
        public double SaturatedFatOrig { get; set; }
        [MapTo("fiberOrig")]
        public double FiberOrig { get; set; }
        [MapTo("saltOrig")]
        public double SaltOrig { get; set; }
        /*[MapTo("food")]
        public Food Food { get; set; }*/

        public Meal()
        {

        }

        public Meal(string name, string brand, double weight, int kcal, double carbs, double sugar,
            double protein, double fat, double saturated, double fiber, double salt, string mealType,
            int kcalOrig, double carbohydratesOrig, double sugarOrig, double proteinOrig, double fatOrig,
            double saturatedFatOrig, double fiberOrig, double saltOrig)
        /*public Meal(string name, string brand, double weight, int kcal, double carbs, double sugar,
            double protein, double fat, double saturated, double fiber, double salt, string mealType,
            Food food)*/
        {
            Name = name;
            Weight = weight;
            Kcal = kcal;
            Brand = brand;
            Carbohydrates = carbs;
            Sugar = sugar;
            Protein = protein;
            Fat = fat;
            SaturatedFat = saturated;
            Fiber = fiber;
            Salt = salt;
            MealType = mealType;
            KcalOrig = kcalOrig;
            CarbohydratesOrig = carbohydratesOrig;
            SugarOrig = sugarOrig;
            ProteinOrig = proteinOrig;
            FatOrig = fatOrig;
            SaturatedFatOrig = saturatedFatOrig;
            FiberOrig = fiberOrig;
            SaltOrig = saltOrig;
            //Food = food;
        }

        /*public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }*/
    }   

    public class MealGroup : ObservableCollection<Meal>
    {
        public string Name { get; set; }
        public string ViewName { get; set; }

        public MealGroup(string name, string viewName, List<Meal> meals) : base(meals)
        {
            Name = name;
            ViewName = viewName;
        }
    }

    public class Breakfast
    {
        public Breakfast(List<Meal> meals)
        {
            Meals = meals;
        }

        public List<Meal> Meals { get; set; }
    }

    public class Lunch
    {
        public Lunch(List<Meal> meals)
        {
            Meals = meals;
        }
        public List<Meal> Meals { get; set; }
    }

    public class Snack
    {
        public Snack(List<Meal> meals)
        {
            Meals = meals;
        }
        public List<Meal> Meals { get; set; }
    }

    public class Dinner
    {
        public Dinner(List<Meal> meals)
        {
            Meals = meals;
        }
        public List<Meal> Meals { get; set; }
    }
}
