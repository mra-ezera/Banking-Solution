namespace Banking.Models.Entities
{
    public class Account
    {
        public Guid Id { get; set; } // Primary Key
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public decimal Balance { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public List<AccountHistory> AccountHistories { get; set; } = new List<AccountHistory>();
    }
}
