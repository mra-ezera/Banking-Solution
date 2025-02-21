using Banking.Models.DTOs;
using Banking.Models.Entities;
using Banking.Models.Results;

namespace Banking.Interfaces
{
    public interface IAccountService
    {
        Task<Account?> GetAccountByIdAsync(Guid id);
        Task UpdateAccountAsync(Account account);
        Task<PagedResultDto<Account>> GetAllAccountsAsync(PaginationParamsDto pagination);
        Task<Result<Account>> CreateAccountAsync(AddAccountDto addAccountDto);
    }
}
