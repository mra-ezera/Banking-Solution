using Banking.Data;
using Banking.Models;
using Banking.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Banking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public TransfersController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost("{fromId:guid}")]
        [SwaggerOperation(Summary = "Transfers an amount from one account to another.")]
        public IActionResult Transfer(Guid fromId, [FromBody] TransferBalanceDto transferDto)
        {
            var fromAccount = dbContext.Accounts.Find(fromId);
            if (fromAccount == null)
            {
                return NotFound("Source account not found.");
            }

            var toAccount = dbContext.Accounts.Find(transferDto.ToAccountId);
            if (toAccount == null)
            {
                return NotFound("Destination account not found.");
            }

            if (fromId == transferDto.ToAccountId)
            {
                return BadRequest("Cannot transfer to the same account.");
            }

            if (fromAccount.Balance < transferDto.Amount)
            {
                return BadRequest("Insufficient funds.");
            }

            if (transferDto.Amount <= 0)
            {
                return BadRequest("Transfer amount must be positive.");
            }

            try
            {
                fromAccount.Balance -= transferDto.Amount;
                toAccount.Balance += transferDto.Amount;

                var now = DateTime.Now;
                fromAccount.DateModified = now;
                toAccount.DateModified = now;

                var fromAccountHistory = new AccountHistory
                {
                    AccountId = fromAccount.Id,
                    TransactionDate = now,
                    Amount = -transferDto.Amount,
                    Description = $"Transfer to {toAccount.Name} {toAccount.Surname}: {transferDto.Description}",
                    Account = fromAccount
                };

                var toAccountHistory = new AccountHistory
                {
                    AccountId = toAccount.Id,
                    TransactionDate = now,
                    Amount = transferDto.Amount,
                    Description = $"Transfer from {fromAccount.Name} {fromAccount.Surname}: {transferDto.Description}",
                    Account = toAccount
                };

                dbContext.AccountHistories.Add(fromAccountHistory);
                dbContext.AccountHistories.Add(toAccountHistory);
                dbContext.SaveChanges();

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                return new JsonResult(new 
                {
                    FromAccount = fromAccount,
                    ToAccount = toAccount
                }, options);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the transfer.");
            }
        }
    }
}
