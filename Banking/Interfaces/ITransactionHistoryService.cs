using Banking.Models.Entities;

namespace Banking.Interfaces
{
    public interface ITransactionHistoryService
    {
        Task AddTransactionHistoryAsync(AccountHistory history);
    }
}
