using System;

namespace FitnessApp01.Helpers
{
    public static class SelectedDay
    {
        private static DateTime Day { get; set; } = DateTime.UtcNow.Date; //je potřeba UTC!

        /// <summary>
        /// Vrátí zvolený den
        /// </summary>
        /// <returns></returns>
        public static DateTime CurrentSelected()
        {
            return Day;
        }

        public static DateTime ActuallyCurrentDay()
        {
            return DateTime.UtcNow.Date;
        }

        /// <summary>
        /// opravdu aktuální den, bez ohledu na to, co je nastaveno (UTC čas)
        /// </summary>
        /// <returns>vrátí unix timestamp ve stringu (UTC čas)</returns>
        public static string ActuallyCurrentDayUnixString()
        {
            return ToUnixSecondsString(ActuallyCurrentDay());
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

        public static string ToUnixSecondsString(DateTime day)
        {
            return new DateTimeOffset(day).ToUnixTimeSeconds().ToString();
        }
    }
}
