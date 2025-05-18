namespace EnsekMeterReadings.Core.Models
{
    public class ProcessReadingsResultModel
    {
        public ProcessReadingsResultModel(int validEntries, int invalidEntries) {
            ValidEntries = validEntries;
            InvalidEntries = invalidEntries;
        }

        public int ValidEntries { get; set; }
        public int InvalidEntries { get; set; }

    }
}
