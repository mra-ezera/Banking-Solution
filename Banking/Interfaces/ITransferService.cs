using Banking.Models;
using Banking.Models.Entities;

namespace Banking.Interfaces
{
    public interface ITransferService
    {
        Task<Result<(Account FromAccount, Account ToAccount)>> TransferAsync(Guid fromId, TransferBalanceDto transferDto);
    }

}
