using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Banking.Interfaces;
using Banking.Models.Results;
using Microsoft.AspNetCore.Authorization;

namespace Banking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransfersController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransfersController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        [HttpPost("{fromId:guid}")]
        [SwaggerOperation(Summary = "Transfers an amount from one account to another.")]
        public async Task<IActionResult> Transfer(Guid fromId, [FromBody] TransferBalanceDto transferDto)
        {
            var result = await _transferService.TransferAsync(fromId, transferDto);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            var transferResult = new TransferResult
            {
                FromAccount = result.Data.FromAccount,
                ToAccount = result.Data.ToAccount
            };

            return Ok(transferResult);
        }
    }
}
