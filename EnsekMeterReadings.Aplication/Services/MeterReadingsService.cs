using CsvHelper;
using EnsekMeterReadings.Aplication.Validation;
using EnsekMeterReadings.Core.Interfaces;
using EnsekMeterReadings.Core.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace EnsekMeterReadings.Aplication.Services
{
    public class MeterReadingsService: IMeterReadingsService
    {
        private readonly IMeterReadingsRepository _meterReadingsRepo;

        public MeterReadingsService(IMeterReadingsRepository meterReadingsRepo)
        {
            _meterReadingsRepo = meterReadingsRepo;
        }

        public async Task<ProcessReadingsResultModel> ProcessCsvEntries(IFormFile file)
        {
            var successEntries = 0;
            var errorEntries = 0;

            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();

            var existingUsers = await _meterReadingsRepo.GetAccountsIds();
            var existingEntries = await _meterReadingsRepo.GetExistingEntries();
            var latestReadings = await _meterReadingsRepo.GetLatestReadings();

            var newEntries = new List<MeterReadingModel>();

            while (await csv.ReadAsync())
            {
                if (!int.TryParse(csv.GetField("AccountId"), out int csvRowAccountId) ||
                    !DateTime.TryParse(csv.GetField("MeterReadingDateTime"), out DateTime csvRowMeterReadDate) ||
                    !int.TryParse(csv.GetField("MeterReadValue"), out int csvRowMeterReadValue)) {
                    errorEntries++;
                    continue;
                }

                var record = new MeterReadingModel
                {
                    AccountId = csvRowAccountId,
                    DateTime = csvRowMeterReadDate,
                    MeterReadValue = csvRowMeterReadValue.ToString("D5"),
                };

                if (EntryValidation.IsValidMeterReading(record, existingUsers, existingEntries, latestReadings))
                {
                    newEntries.Add(record);
                    existingEntries.Add((record.AccountId, record.DateTime));

                    if (latestReadings.TryGetValue(record.AccountId, out var latest))
                    {
                        if (record.DateTime > latest)
                            latestReadings[record.AccountId] = record.DateTime;
                    }
                    else
                    {
                        latestReadings[record.AccountId] = record.DateTime;
                    }

                    successEntries++;
                }
                else
                {
                    errorEntries++;
                }
            }

            if (newEntries.Any())
            {
                await _meterReadingsRepo.AddMeterReadings(newEntries);
            }

            return new ProcessReadingsResultModel(successEntries, errorEntries);
        }

        public async Task<List<MeterReadingModel>> GetMeterReadings()
        {
            var latestReadings = await _meterReadingsRepo.GetAllMeterReadings();

            return latestReadings;
        }
    }
}
