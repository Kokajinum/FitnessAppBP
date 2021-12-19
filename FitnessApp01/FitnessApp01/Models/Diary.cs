using System;
using System.Collections.ObjectModel;

namespace FitnessApp01.Models
{
    public static class Diary
    {
        public static ObservableCollection<Day> Days { get; set; } = new ObservableCollection<Day>();

        public static RegistrationSettings RegistrationSettings { get; set; }

        public static Day GetSelectedDay()
        {
            try
            {

            } 
            catch(Exception ex)
            {

            }
            return new Day();
        }

        //[MapTo("UnixSeconds")]
        //public Timestamp Timestamp { get; set; }
        //public int CaloriesGoal { get; set; }
        //public int CaloriesCurrent { get; set; }
        //public MealsOfTheDay Meals { get; set; }
    }
}
