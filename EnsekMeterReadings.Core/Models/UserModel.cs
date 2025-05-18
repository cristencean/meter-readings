namespace EnsekMeterReadings.Core.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}