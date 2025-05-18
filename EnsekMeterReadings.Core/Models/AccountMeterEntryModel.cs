namespace EnsekMeterReadings.Core.Models
{
    public class AccountMeterEntryModel
    {
        public AccountMeterEntryModel(int accountId, DateTime dateTime) { 
            AccountId = accountId;
            DateTime = dateTime;
        }

        public int AccountId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
