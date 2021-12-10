using Plugin.CloudFirestore.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FitnessApp01.Models
{
    public class Day
    {
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
        public IDictionary<string, double> Macros { get; set; } = new Dictionary<string, double>
        {
            {"carbohydrates", 0 },
            {"protein", 0 },
            {"fat", 0 }
        };

        [Ignored]
        public ObservableCollection<MealGroup> MealGroups { get; set; } = new ObservableCollection<MealGroup>();

        public Day Clone()
        {
            return (Day)this.MemberwiseClone();
        }
    }
}
