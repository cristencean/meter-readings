using EnsekMeterReadings.Core.Interfaces;
using EnsekMeterReadings.Core.Models;
using EnsekMeterReadings.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace EnsekMeterReadings.DataAccess.Repositories
{
    public class MeterReadingsRepository : IMeterReadingsRepository
    {
        private readonly ApplicationDbContext _context;

        public MeterReadingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HashSet<int>> GetAccountsIds()
        {
            return await _context.Users
                .Select(u => u.AccountId)
                .ToHashSetAsync();
        }

        public async Task<HashSet<(int, DateTime)>> GetExistingEntries()
        {
            return await _context.MeterReadings
                .Select(m => new { m.AccountId, m.DateTime })
                .AsNoTracking()
                .ToListAsync()
                .ContinueWith(t => t.Result
                    .Select(x => (x.AccountId, x.DateTime))
                    .ToHashSet());
        }

        public async Task<Dictionary<int, DateTime>> GetLatestReadings()
        {
            return await _context.MeterReadings
                .GroupBy(m => m.AccountId)
                .Select(g => new { g.Key, MaxDate = g.Max(m => m.DateTime) })
                .ToDictionaryAsync(x => x.Key, x => x.MaxDate);
        }

        public async Task AddMeterReadings(List<MeterReadingModel> readings)
        {
            _context.MeterReadings.AddRange(readings);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MeterReadingModel>> GetAllMeterReadings()
        {
            return await _context.MeterReadings
                .Select(m => new MeterReadingModel()
                {
                    Id = m.Id,
                    AccountId = m.AccountId,
                    DateTime = m.DateTime,
                    MeterReadValue = m.MeterReadValue
                })
                .ToListAsync();
        }
    }
}