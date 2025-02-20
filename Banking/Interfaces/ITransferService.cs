using Banking.Models.Entities;
using Banking.Models.Results;

namespace Banking.Interfaces
{
    public interface ITransferService
    {
        Task<Result<(Account FromAccount, Account ToAccount)>> TransferAsync(Guid fromId, TransferBalanceDto transferDto);
    }

}
