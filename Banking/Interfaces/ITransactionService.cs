using Banking.Models.DTOs;
using Banking.Models.Entities;
using Banking.Models.Results;

namespace Banking.Interfaces
{
    public interface ITransactionService
    {
        Task<Result<Account>> AddBalanceAsync(Guid accountId, UpdateBalanceDto updateBalanceDto);
        Task<Result<Account>> RemoveBalanceAsync(Guid accountId, UpdateBalanceDto updateBalanceDto);
    }
}
