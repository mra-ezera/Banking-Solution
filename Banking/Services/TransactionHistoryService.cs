using Banking.Data;
using Banking.Interfaces;
using Banking.Models.Entities;

namespace Banking.Services
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly ApplicationDbContext _context;

        public TransactionHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTransactionHistoryAsync(AccountHistory history)
        {
            _context.AccountHistories.Add(history);
            await _context.SaveChangesAsync();
        }
    }
}
