using EnsekMeterReadings.Core.Models;
using System.Text.RegularExpressions;

namespace EnsekMeterReadings.Aplication.Validation
{
    public static class EntryValidation
    {
        public static bool IsValidMeterReading(
            MeterReadingModel record,
            HashSet<int> existingUsers,
            HashSet<(int AccountId, DateTime DateTime)> existingEntries,
            Dictionary<int, DateTime> latestReadings)
        {
            if (!existingUsers.Contains(record.AccountId))
                return false;

            if (existingEntries.Contains((record.AccountId, record.DateTime)))
                return false;

            if (latestReadings.TryGetValue(record.AccountId, out var latest))
                if (record.DateTime <= latest)
                    return false;

            if (!Regex.IsMatch(record.MeterReadValue, @"^\d{5}$"))
                return false;

            return true;
        }
    }
}
