using Newtonsoft.Json;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
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
