using Banking.Interfaces;
using Banking.Models.Entities;
using System.Threading.Tasks;

namespace Banking.Services
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        public async Task AddTransactionHistoryAsync(AccountHistory history)
        {
            await Task.CompletedTask;
        }
    }
}
