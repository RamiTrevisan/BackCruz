using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Checks if a date is between two other dates
        /// </summary>
        public static bool IsBetween(this DateTime date, DateTime start, DateTime end)
        {
            return date >= start && date <= end;
        }

        /// <summary>
        /// Gets a friendly date string (e.g. "Today", "Yesterday", etc.)
        /// </summary>
        public static string ToFriendlyDateString(this DateTime date)
        {
            var now = DateTime.Now;
            var today = now.Date;
            var yesterday = today.AddDays(-1);
            var tomorrow = today.AddDays(1);

            if (date.Date == today)
                return "Hoy";
            if (date.Date == yesterday)
                return "Ayer";
            if (date.Date == tomorrow)
                return "Mañana";

            // If it's in the same year, don't show the year
            if (date.Year == now.Year)
                return date.ToString("d 'de' MMMM");

            return date.ToString("d 'de' MMMM 'de' yyyy");
        }
    }
}
