using Moq;
using EnsekMeterReadings.Aplication.Services;
using EnsekMeterReadings.Core.Interfaces;
using EnsekMeterReadings.Core.Models;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace EnsekMeterReadings.Tests.Application
{
    public class MeterReadingsServiceTests
    {
        private readonly Mock<IMeterReadingsRepository> _mockRepo;
        private readonly MeterReadingsService _service;

        public MeterReadingsServiceTests()
        {
            _mockRepo = new Mock<IMeterReadingsRepository>();
            _service = new MeterReadingsService(_mockRepo.Object);
        }

        [Fact]
        public async Task ProcessCsvEntries_Returns_CorrectCounts_ForValidAndInvalidRows()
        {
            var csvContent = new StringBuilder();
            csvContent.AppendLine("AccountId,MeterReadingDateTime,MeterReadValue");
            csvContent.AppendLine("2394,22/04/2025 09:24,01012");
            csvContent.AppendLine("INVALID,22/04/2025 09:24,01012");
            csvContent.AppendLine("2394,INVALID_DATE,01012");
            csvContent.AppendLine("2394,22/04/2025 09:24,WRONG");

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent.ToString()));
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            formFileMock.Setup(f => f.FileName).Returns("meterReadings.csv");
            formFileMock.Setup(f => f.Length).Returns(stream.Length);

            var accountIds = new HashSet<int> { 2394 };
            var existingEntries = new HashSet<(int, DateTime)>();
            var latestReadings = new Dictionary<int, DateTime>();

            _mockRepo.Setup(r => r.GetAccountsIds()).ReturnsAsync(accountIds);
            _mockRepo.Setup(r => r.GetExistingEntries()).ReturnsAsync(existingEntries);
            _mockRepo.Setup(r => r.GetLatestReadings()).ReturnsAsync(latestReadings);

            var savedEntries = new List<MeterReadingModel>();
            _mockRepo.Setup(r => r.AddMeterReadings(It.IsAny<List<MeterReadingModel>>()))
                .Callback<List<MeterReadingModel>>(entries => savedEntries.AddRange(entries))
                .Returns(Task.CompletedTask);

            var result = await _service.ProcessCsvEntries(formFileMock.Object);

            Assert.Equal(1, result.ValidEntries);
            Assert.Equal(3, result.InvalidEntries);
            Assert.Single(savedEntries);
            Assert.Equal("01012", savedEntries[0].MeterReadValue);
        }
    }
}