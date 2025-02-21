using Banking.Interfaces;
using Banking.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Banking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("{id:guid}/deposit")]
        [SwaggerOperation(Summary = "Deposits an amount to the account balance.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Deposit(Guid id, [FromBody] UpdateBalanceDto updateBalanceDto)
        {
            var result = await _transactionService.AddBalanceAsync(id, updateBalanceDto);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [HttpPost("{id:guid}/withdraw")]
        [SwaggerOperation(Summary = "Withdraws an amount from the account balance.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Withdraw(Guid id, [FromBody] UpdateBalanceDto updateBalanceDto)
        {
            var result = await _transactionService.RemoveBalanceAsync(id, updateBalanceDto);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }
    }
}