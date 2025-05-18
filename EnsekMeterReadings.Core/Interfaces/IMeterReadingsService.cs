using EnsekMeterReadings.Core.Models;
using Microsoft.AspNetCore.Http;

namespace EnsekMeterReadings.Core.Interfaces
{
    public interface IMeterReadingsService
    {
        Task<ProcessReadingsResultModel> ProcessCsvEntries(IFormFile file);
        Task<List<MeterReadingModel>> GetMeterReadings();
    }
}
