using Banking.Data;
using Banking.Interfaces;
using Banking.Models;
using Banking.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account?> GetAccountByIdAsync(Guid id)
            => await _dbContext.Accounts.FindAsync(id);

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
            => await _dbContext.Accounts.ToListAsync();

        public async Task<Result<Account>> CreateAccountAsync(AddAccountDto addAccountDto)
        {
            var account = new Account
            {
                Name = addAccountDto.Name,
                Surname = addAccountDto.Surname,
                Email = addAccountDto.Email,
                Balance = 0, // Initial balance is zero
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                AccountHistories = new List<AccountHistory>()
            };

            try
            {
                _dbContext.Accounts.Add(account);
                await _dbContext.SaveChangesAsync();
                return Result<Account>.Success(account);
            }
            catch (Exception ex)
            {
                return Result<Account>.Failure($"Failed to create account: {ex.Message}");
            }
        }

        public async Task UpdateAccountAsync(Account account)
        {
            _dbContext.Accounts.Update(account);
            await _dbContext.SaveChangesAsync();
        }
    }
}