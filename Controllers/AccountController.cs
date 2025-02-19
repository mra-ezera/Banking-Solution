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
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public AccountController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all accounts.")]
        public IActionResult GetAccounts()
        {
            var allAccounts = dbContext.Accounts.ToList();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(allAccounts, options);
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(Summary = "Gets an account by ID.")]
        public IActionResult GetAccountById(Guid id)
        {
            var account = dbContext.Accounts.Find(id);

            if (account == null)
            {
                return NotFound();
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(account, options);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Adds a new account.")]
        public IActionResult AddAccount(AddAccountDto addAccountDto)
        {
            var accountEntity = new Account()
            {
                Name = addAccountDto.Name,
                Surname = addAccountDto.Surname,
                Email = addAccountDto.Email,
                Balance = addAccountDto.Balance,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            dbContext.Accounts.Add(accountEntity);
            dbContext.SaveChanges();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(accountEntity, options);
        }
    }
}