public class TransferBalanceDto
{
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
}
