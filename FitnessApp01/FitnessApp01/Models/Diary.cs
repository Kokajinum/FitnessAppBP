using System.Collections.ObjectModel;

namespace FitnessApp01.Models
{
    public static class Diary
    {
        //public static List<Day> Days { get; set; }
        public static ObservableCollection<Day> Days { get; set; }



        //[MapTo("UnixSeconds")]
        //public Timestamp Timestamp { get; set; }
        //public int CaloriesGoal { get; set; }
        //public int CaloriesCurrent { get; set; }
        //public MealsOfTheDay Meals { get; set; }
    }
}
