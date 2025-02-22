using Banking.Interfaces;
using Banking.Models.DTOs;
using Banking.Models.Results;
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
            if (updateBalanceDto.Amount <= 0)
            {
                return BadRequest(new ErrorResponse { Error = "Deposit amount must be greater than zero." });
            }

            try
            {
                var result = await _transactionService.AddBalanceAsync(id, updateBalanceDto);
                if (!result.IsSuccess)
                    return BadRequest(result.Error);

                return Ok(result.Data);
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse { Error = "An error occurred while processing your request" });
            }
        }

        [HttpPost("{id:guid}/withdraw")]
        [SwaggerOperation(Summary = "Withdraws an amount from the account balance.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Withdraw(Guid id, [FromBody] UpdateBalanceDto updateBalanceDto)
        {
            if (updateBalanceDto.Amount <= 0)
            {
                return BadRequest(new ErrorResponse { Error = "Withdrawal amount must be greater than zero." });
            }

            try
            {
                var result = await _transactionService.RemoveBalanceAsync(id, updateBalanceDto);
                if (!result.IsSuccess)
                    return BadRequest(result.Error);

                return Ok(result.Data);
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse { Error = "An error occurred while processing your request" });
            }
        }
    }
}
