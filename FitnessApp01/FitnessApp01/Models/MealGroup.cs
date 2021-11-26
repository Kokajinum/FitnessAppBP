using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FitnessApp01.Models
{
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
}
