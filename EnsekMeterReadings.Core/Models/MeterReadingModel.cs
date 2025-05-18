namespace EnsekMeterReadings.Core.Models
{
    public class MeterReadingModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string MeterReadValue { get; set; } = "00000";
        public DateTime DateTime { get; set; }
    }
}