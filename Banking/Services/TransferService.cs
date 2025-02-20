using Banking.Interfaces;
using Banking.Models.Entities;
using Banking.Models.Results;
using System;
using System.Threading.Tasks;

namespace Banking.Services
{
    public class TransferService : ITransferService
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionHistoryService _historyService;

        public TransferService(IAccountService accountService, ITransactionHistoryService historyService)
        {
            _accountService = accountService;
            _historyService = historyService;
        }

        public async Task<Result<(Account FromAccount, Account ToAccount)>> TransferAsync(Guid fromId, TransferBalanceDto transferDto)
        {
            var fromAccount = await _accountService.GetAccountByIdAsync(fromId);
            var toAccount = await _accountService.GetAccountByIdAsync(transferDto.ToAccountId);

            if (fromAccount == null || toAccount == null)
                return Result<(Account, Account)>.Failure("One or both accounts not found.");

            if (fromAccount.Balance < transferDto.Amount)
                return Result<(Account, Account)>.Failure("Insufficient funds.");

            fromAccount.Balance -= transferDto.Amount;
            toAccount.Balance += transferDto.Amount;

            var fromAccountHistory = new AccountHistory
            {
                AccountId = fromAccount.Id,
                TransactionDate = DateTime.Now,
                Amount = -transferDto.Amount,
                Description = transferDto.Description,
                Account = fromAccount
            };

            var toAccountHistory = new AccountHistory
            {
                AccountId = toAccount.Id,
                TransactionDate = DateTime.Now,
                Amount = transferDto.Amount,
                Description = transferDto.Description,
                Account = toAccount
            };

            await _historyService.AddTransactionHistoryAsync(fromAccountHistory);
            await _historyService.AddTransactionHistoryAsync(toAccountHistory);

            await _accountService.UpdateAccountAsync(fromAccount);
            await _accountService.UpdateAccountAsync(toAccount);

            return Result<(Account, Account)>.Success((fromAccount, toAccount));
        }
    }
}