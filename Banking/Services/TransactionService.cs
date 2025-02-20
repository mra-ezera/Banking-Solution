using Banking.Interfaces;
using Banking.Models.Entities;
using Banking.Models.Results;
using Banking.Models.DTOs;

namespace Banking.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionHistoryService _historyService;

        public TransactionService(IAccountService accountService, ITransactionHistoryService historyService)
        {
            _accountService = accountService;
            _historyService = historyService;
        }

        public async Task<Result<Account>> AddBalanceAsync(Guid accountId, UpdateBalanceDto updateBalanceDto)
        {
            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account == null)
                return Result<Account>.Failure("Account not found.");

            account.Balance += updateBalanceDto.Amount;
            account.DateModified = DateTime.Now;

            var accountHistory = new AccountHistory
            {
                AccountId = account.Id,
                TransactionDate = DateTime.Now,
                Amount = updateBalanceDto.Amount,
                Description = updateBalanceDto.Description,
                Account = account
            };

            await _historyService.AddTransactionHistoryAsync(accountHistory);
            await _accountService.UpdateAccountAsync(account);

            return Result<Account>.Success(account);
        }

        public async Task<Result<Account>> RemoveBalanceAsync(Guid accountId, UpdateBalanceDto updateBalanceDto)
        {
            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account == null)
                return Result<Account>.Failure("Account not found.");

            if (account.Balance < updateBalanceDto.Amount)
                return Result<Account>.Failure("Insufficient funds.");

            account.Balance -= updateBalanceDto.Amount;
            account.DateModified = DateTime.Now;

            var accountHistory = new AccountHistory
            {
                AccountId = account.Id,
                TransactionDate = DateTime.Now,
                Amount = -updateBalanceDto.Amount,
                Description = updateBalanceDto.Description,
                Account = account
            };

            await _historyService.AddTransactionHistoryAsync(accountHistory);
            await _accountService.UpdateAccountAsync(account);

            return Result<Account>.Success(account);
        }
    }
}
