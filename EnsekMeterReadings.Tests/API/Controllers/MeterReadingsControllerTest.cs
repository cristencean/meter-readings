using Moq;
using Microsoft.AspNetCore.Mvc;
using EnsekMeterReadings.Api.Controllers;
using EnsekMeterReadings.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using EnsekMeterReadings.Core.Models;

namespace EnsekMeterReadings.Tests.API
{
    public class MeterReadingsControllerTests
    {
        private readonly Mock<IMeterReadingsService> _mockMeterReadingsService;
        private readonly MeterReadingsController _controller;

        public MeterReadingsControllerTests()
        {
            _mockMeterReadingsService = new Mock<IMeterReadingsService>();
            _controller = new MeterReadingsController(_mockMeterReadingsService.Object);
        }

        [Fact]
        public async Task ProcessMeterReadings_ReturnsBadRequest_WhenNoFileUploaded()
        {
            IFormFile file = null;

            var result = await _controller.ProcessMeterReadings(file);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No file uploaded.", badRequestResult.Value);
        }

        [Fact]
        public async Task ProcessMeterReadings_ReturnsOk_WhenValidFileUploaded()
        {
            var fileMock = new Mock<IFormFile>();
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("Test content");
            writer.Flush();
            stream.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            fileMock.Setup(f => f.FileName).Returns("meterReadings.csv");
            fileMock.Setup(f => f.Length).Returns(stream.Length);

            var expectedResult = new ProcessReadingsResultModel(15, 7);

            _mockMeterReadingsService.Setup(s => s.ProcessCsvEntries(It.IsAny<IFormFile>())).ReturnsAsync(expectedResult);

            var result = await _controller.ProcessMeterReadings(fileMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProcessReadingsResultModel>(okResult.Value);
            Assert.Equal(expectedResult, returnValue);
        }

        [Fact]
        public async Task ProcessMeterReadings_ReturnsBadRequest_WhenFileHasWrongFormat()
        {
            var fileMock = new Mock<IFormFile>();
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("Invalid content");
            writer.Flush();
            stream.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            fileMock.Setup(f => f.FileName).Returns("wrong-file.txt");
            fileMock.Setup(f => f.Length).Returns(stream.Length);

            _mockMeterReadingsService.Setup(s => s.ProcessCsvEntries(It.IsAny<IFormFile>()))
                .ThrowsAsync(new FakeHeaderValidationException());

            var result = await _controller.ProcessMeterReadings(fileMock.Object);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The file has wrong format. It should be a CSV file.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetMeterReadings_ReturnsOk_WhenServiceReturnsData()
        {
            var expectedReadings = new List<MeterReadingModel>()
            {
                new MeterReadingModel()
                {
                    Id = 1,
                    AccountId = 1463,
                    MeterReadValue = "00453",
                    DateTime = DateTime.UtcNow,
                }
            };

            _mockMeterReadingsService.Setup(s => s.GetMeterReadings()).ReturnsAsync(expectedReadings);

            var result = await _controller.GetMeterReadings();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<MeterReadingModel>>(okResult.Value);
            Assert.Equal(expectedReadings.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetMeterReadings_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            _mockMeterReadingsService.Setup(s => s.GetMeterReadings())
                .ThrowsAsync(new Exception("Something went wrong"));

            var result = await _controller.GetMeterReadings();

            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
