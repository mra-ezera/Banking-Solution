namespace Banking.Models.Entities
{
    public class AccountHistory
    {
        public int Id { get; set; }
        public Guid AccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public required Account Account { get; set; }
    }
}
