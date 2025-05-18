using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using EnsekMeterReadings.Aplication.Services;
using EnsekMeterReadings.Core.Interfaces;
using EnsekMeterReadings.Core.Models;

namespace EnsekMeterReadings.Api.Controllers
{
    [Route("")]
    [ApiController]
    public class MeterReadingsController : ControllerBase
    {
        private IMeterReadingsService _meterReadingsService {  get; set; }

        public MeterReadingsController(IMeterReadingsService meterReadingsService)
        {
            _meterReadingsService = meterReadingsService;
        }

        [HttpPost("meter-readings-uploads")]
        public async Task<IActionResult> ProcessMeterReadings(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                var result = await _meterReadingsService.ProcessCsvEntries(file);

                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex is HeaderValidationException || ex is FakeHeaderValidationException)
                {
                    return BadRequest("The file has wrong format. It should be a CSV file.");
                }

                return StatusCode(500, $"An error occured while processing the entries: {ex.Message}");
            }
        }

        [HttpGet("meter-readings")]
        public async Task<IActionResult> GetMeterReadings()
        {
            try
            {
                var result = await _meterReadingsService.GetMeterReadings();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while getting the meter readins: {ex.Message}");
            }
        }
    }
}
