using Banking.Data;
using Banking.Interfaces;
using Banking.Models.DTOs;
using Banking.Models.Entities;
using Banking.Models.Results;
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
        {
            return await _dbContext.Accounts
                .Include(a => a.AccountHistories)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
            => await _dbContext.Accounts.ToListAsync();

        public async Task<Result<Account>> CreateAccountAsync(AddAccountDto addAccountDto)
        {
            var account = new Account
            {
                Name = addAccountDto.Name,
                Surname = addAccountDto.Surname,
                Email = addAccountDto.Email,
                Balance = addAccountDto.Balance,
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

        public async Task<PagedResultDto<Account>> GetAllAccountsAsync(PaginationParamsDto pagination)
        {
            var query = _dbContext.Accounts.AsQueryable();

            var totalItems = await query.CountAsync();
            var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                                   .Take(pagination.PageSize)
                                   .ToListAsync();

            return new PagedResultDto<Account>
            {
                Items = items,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalCount = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pagination.PageSize),
                HasNextPage = pagination.PageNumber < (int)Math.Ceiling(totalItems / (double)pagination.PageSize),
                HasPreviousPage = pagination.PageNumber > 1
            };
        }
    }
}
