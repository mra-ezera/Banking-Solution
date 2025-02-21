using Banking.Interfaces;
using Banking.Models.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
