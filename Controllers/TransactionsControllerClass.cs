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
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public TransactionsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost("{id:guid}/add")]
        [SwaggerOperation(Summary = "Adds an amount to the account balance.")]
        public IActionResult AddToBalance(Guid id, [FromBody] UpdateBalanceDto updateBalanceDto)
        {
            var account = dbContext.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }

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

            dbContext.AccountHistories.Add(accountHistory);
            dbContext.SaveChanges();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(account, options);
        }

        [HttpPost("{id:guid}/remove")]
        [SwaggerOperation(Summary = "Removes an amount from the account balance.")]
        public IActionResult RemoveFromBalance(Guid id, [FromBody] UpdateBalanceDto updateBalanceDto)
        {
            var account = dbContext.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }

            if (account.Balance < updateBalanceDto.Amount)
            {
                return BadRequest("Insufficient funds.");
            }

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

            dbContext.AccountHistories.Add(accountHistory);
            dbContext.SaveChanges();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(account, options);
        }
    }
}