using Banking.Interfaces;
using Banking.Models.DTOs;
using Banking.Models.Entities;
using Banking.Models.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Banking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all accounts with pagination.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResultDto<Account>>> GetAll([FromQuery] PaginationParamsDto pagination)
        {
            try
            {
                var pagedAccounts = await _accountService.GetAllAccountsAsync(pagination);
                return Ok(pagedAccounts);
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse { Error = "An error occurred while processing your request" });
            }
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(Summary = "Gets an account by ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var account = await _accountService.GetAccountByIdAsync(id);
                if (account == null)
                    return NotFound();
                return Ok(account);
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse { Error = "An error occurred while processing your request" });
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new account.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] AddAccountDto addAccountDto)
        {
            try
            {
                var result = await _accountService.CreateAccountAsync(addAccountDto);

                if (!result.IsSuccess)
                    return BadRequest(result.Error);

                if (result.Data == null)
                    return BadRequest("Account creation failed.");

                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse { Error = "An error occurred while processing your request" });
            }
        }
    }
}
