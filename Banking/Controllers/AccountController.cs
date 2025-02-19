using Banking.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Banking.Interfaces;
using Banking.Models;

namespace Banking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets an account by ID.")]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(Summary = "Gets an account by ID.")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
                return NotFound();
            return Ok(account);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new account.")]
        public async Task<IActionResult> Create([FromBody] AddAccountDto addAccountDto)
        {
            var result = await _accountService.CreateAccountAsync(addAccountDto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
        }
    }
}