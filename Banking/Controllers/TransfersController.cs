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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Transfer(Guid fromId, [FromBody] TransferBalanceDto transferDto)
        {
            if (transferDto.Amount <= 0)
            {
                return BadRequest(new ErrorResponse { Error = "Transfer amount must be greater than zero." });
            }

            try
            {
                var result = await _transferService.TransferAsync(fromId, transferDto);
                if (!result.IsSuccess)
                    return BadRequest(new ErrorResponse { Error = result.Error ?? "Unknown error" });

                var transferResult = new TransferResult
                {
                    FromAccount = result.Data.FromAccount,
                    ToAccount = result.Data.ToAccount
                };

                return Ok(transferResult);
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse { Error = "An error occurred while processing your request" });
            }
        }
    }
}

