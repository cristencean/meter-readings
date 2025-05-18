using EnsekMeterReadings.Core.Models;

namespace EnsekMeterReadings.Core.Interfaces
{
    public interface IMeterReadingsRepository
    {
        Task<HashSet<int>> GetAccountsIds();
        Task<HashSet<(int AccountId, DateTime DateTime)>> GetExistingEntries();
        Task<Dictionary<int, DateTime>> GetLatestReadings();
        Task AddMeterReadings(List<MeterReadingModel> readings);
        Task<List<MeterReadingModel>> GetAllMeterReadings();
    }
}