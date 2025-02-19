using Banking.Models;
using Banking.Models.Entities;

namespace Banking.Interfaces
{
    public interface IAccountService
    {
        Task<Account?> GetAccountByIdAsync(Guid id);
        Task UpdateAccountAsync(Account account);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Result<Account>> CreateAccountAsync(AddAccountDto addAccountDto);
    }
}
