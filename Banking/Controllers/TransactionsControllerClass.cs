using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Banking.Interfaces;
using Banking.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost("{id:guid}/add")]
        [SwaggerOperation(Summary = "Adds an amount to the account balance.")]
        public async Task<IActionResult> AddToBalance(Guid id, [FromBody] UpdateBalanceDto updateBalanceDto)
        {
            var result = await _transactionService.AddBalanceAsync(id, updateBalanceDto);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [HttpPost("{id:guid}/remove")]
        [SwaggerOperation(Summary = "Removes an amount from the account balance.")]
        public async Task<IActionResult> RemoveFromBalance(Guid id, [FromBody] UpdateBalanceDto updateBalanceDto)
        {
            var result = await _transactionService.RemoveBalanceAsync(id, updateBalanceDto);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }
    }
}