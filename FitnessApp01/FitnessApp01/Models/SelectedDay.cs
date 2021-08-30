using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FitnessApp01.Models
{
    public static class SelectedDay
    {
        public static DateTime Day { get; set; } = DateTime.UtcNow.Date; //je potřeba UTC!

        public static DateTime Current()
        {
            return Day;
        }


        /// <summary>
        /// Adding 1 day to property Day and returning Day
        /// </summary>
        /// <returns></returns>
        public static DateTime Next()
        {
            Day = Day.AddDays(1);
            return Day;
        }

        public static DateTime Previous()
        {
            Day = Day.AddDays(-1);
            return Day;
        }

        public static string ToUnixSecondsString()
        {
            return new DateTimeOffset(Day).ToUnixTimeSeconds().ToString();
        }
    }
}
