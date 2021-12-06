using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FitnessApp01.Models
{
    public static class SelectedDay
    {
        public static DateTime Day { get; set; } = DateTime.UtcNow.Date; //je potřeba UTC!

        /// <summary>
        /// Vrátí aktuální den
        /// </summary>
        /// <returns></returns>
        public static DateTime Current()
        {
            return Day;
        }


        /// <summary>
        /// Posun na další den
        /// </summary>
        /// <returns>Vrátí další den</returns>
        public static DateTime Next()
        {
            Day = Day.AddDays(1);
            return Day;
        }

        /// <summary>
        /// Posun na předchozí den
        /// </summary>
        /// <returns>Vrátí předchozí den</returns>
        public static DateTime Previous()
        {
            Day = Day.AddDays(-1);
            return Day;
        }

        /// <summary>
        /// Vrátí aktuálně nastavený den ve formátu string unix timestampu
        /// </summary>
        /// <returns></returns>
        public static string ToUnixSecondsString()
        {
            return new DateTimeOffset(Day).ToUnixTimeSeconds().ToString();
        }
    }
}
