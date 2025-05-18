using EnsekMeterReadings.Core.Models;

namespace EnsekMeterReadings.Aplication.Validation
{
    public class EntryValidationTests
    {
        private readonly HashSet<int> _existingUsers = new() { 1001, 1002 };
        private readonly HashSet<(int, DateTime)> _existingEntries = new();
        private readonly Dictionary<int, DateTime> _latestReadings = new();

        [Fact]
        public void IsValidMeterReading_ReturnsTrue_ForValidReading()
        {
            var record = new MeterReadingModel
            {
                AccountId = 1001,
                DateTime = new DateTime(2024, 1, 1),
                MeterReadValue = "01234"
            };

            var result = EntryValidation.IsValidMeterReading(record, _existingUsers, _existingEntries, _latestReadings);

            Assert.True(result);
        }

        [Fact]
        public void IsValidMeterReading_ReturnsFalse_WhenUserDoesNotExist()
        {
            var record = new MeterReadingModel
            {
                AccountId = 9999,
                DateTime = DateTime.Now,
                MeterReadValue = "01234"
            };

            var result = EntryValidation.IsValidMeterReading(record, _existingUsers, _existingEntries, _latestReadings);

            Assert.False(result);
        }

        [Fact]
        public void IsValidMeterReading_ReturnsFalse_WhenEntryAlreadyExists()
        {
            var date = new DateTime(2024, 1, 1);
            _existingEntries.Add((1001, date));

            var record = new MeterReadingModel
            {
                AccountId = 1001,
                DateTime = date,
                MeterReadValue = "01234"
            };

            var result = EntryValidation.IsValidMeterReading(record, _existingUsers, _existingEntries, _latestReadings);

            Assert.False(result);
        }

        [Fact]
        public void IsValidMeterReading_ReturnsFalse_WhenReadingIsOlderThanLatest()
        {
            var latestDate = new DateTime(2024, 5, 1);
            _latestReadings[1001] = latestDate;

            var record = new MeterReadingModel
            {
                AccountId = 1001,
                DateTime = new DateTime(2024, 4, 1),
                MeterReadValue = "01234"
            };

            var result = EntryValidation.IsValidMeterReading(record, _existingUsers, _existingEntries, _latestReadings);

            Assert.False(result);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("abcde")]
        [InlineData("123456")]
        [InlineData("12 34")]
        public void IsValidMeterReading_ReturnsFalse_WhenReadValueIsInvalid(string value)
        {
            var record = new MeterReadingModel
            {
                AccountId = 1001,
                DateTime = new DateTime(2024, 1, 1),
                MeterReadValue = value
            };

            var result = EntryValidation.IsValidMeterReading(record, _existingUsers, _existingEntries, _latestReadings);

            Assert.False(result);
        }
    }
}