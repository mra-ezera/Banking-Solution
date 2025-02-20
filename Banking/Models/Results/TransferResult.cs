using Banking.Models.Entities;

namespace Banking.Models.Results
{
    public class TransferResult
    {
        public required Account FromAccount { get; set; }
        public required Account ToAccount { get; set; }
    }
}
