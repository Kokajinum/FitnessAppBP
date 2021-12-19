using FitnessApp01.Resx;
using Plugin.CloudFirestore.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FitnessApp01.Models
{
    public class Day
    {
        public Day()
        {
            UnixSeconds = SelectedDay.ToUnixSecondsString();
            Macros = new Dictionary<string, double>
            {
                {"carbohydrates", 0 },
                {"protein", 0 },
                {"fat", 0 }
            };
            MealGroups = new ObservableCollection<MealGroup>();
            /*if (MealGroups.Count == 0)
            {
                MealGroups.Add(new MealGroup("breakfast", AppResources.Breakfast, new List<Meal>()));
                MealGroups.Add(new MealGroup("lunch", AppResources.Lunch, new List<Meal>()));
                MealGroups.Add(new MealGroup("snack", AppResources.Snack, new List<Meal>()));
                MealGroups.Add(new MealGroup("dinner", AppResources.Dinner, new List<Meal>()));
            }*/
        }

        [MapTo("unixSeconds")]
        public string UnixSeconds { get; set; }
        [MapTo("caloriesGoal")]
        public int CaloriesGoal { get; set; }
        [MapTo("caloriesCurrent")]
        public int CaloriesCurrent { get; set; }
        [MapTo("protein")]
        public double Protein { get; set; }
        [MapTo("carbohydrates")]
        public double Carbohydrates { get; set; }
        [MapTo("sugar")]
        public double Sugar { get; set; }
        [MapTo("fat")]
        public double Fat { get; set; }
        [MapTo("saturatedFat")]
        public double SaturatedFat { get; set; }
        [MapTo("fiber")]
        public double Fiber { get; set; }
        [MapTo("salt")]
        public double Salt { get; set; }
        [MapTo("macros")]
        public IDictionary<string, double> Macros { get; set; }

        [Ignored]
        public ObservableCollection<MealGroup> MealGroups { get; set; }

        public Day Clone()
        {
            return (Day)this.MemberwiseClone();
        }
    }
}
