namespace Banking.Models.DTOs
{
    public class UpdateBalanceDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
