using Banking.Data;
using Banking.Models;
using Banking.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using Banking.Interfaces;

namespace Banking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

            return Ok(new { FromAccount = result.Data.FromAccount, ToAccount = result.Data.ToAccount });
        }
    }
}
