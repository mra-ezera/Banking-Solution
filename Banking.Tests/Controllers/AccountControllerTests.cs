using Banking.Controllers;
using Banking.Interfaces;
using Banking.Models;
using Banking.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Banking.Tests.Controllers
{
    public class AccountControllerTests
    {
        [Fact]
        public async Task Create_ValidData_ReturnsCreatedResult()
        {
            var mockService = new Mock<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var dto = new AddAccountDto
            {
                Name = "John",
                Surname = "Doe",
                Email = "john.doe@example.com"
            };

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email
            };

            mockService
                .Setup(s => s.CreateAccountAsync(It.IsAny<AddAccountDto>()))
                .ReturnsAsync(Result<Account>.Success(account));

            var result = await controller.Create(dto);

            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedAccount = Assert.IsType<Account>(createdAtResult.Value);
            Assert.Equal(account.Id, returnedAccount.Id);
        }

        [Fact]
        public async Task GetById_ExistingAccount_ReturnsOkResult()
        {
            var mockService = new Mock<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Name = "John",
                Surname = "Doe",
                Email = "john.doe@example.com"
            };

            mockService
                .Setup(s => s.GetAccountByIdAsync(account.Id))
                .ReturnsAsync(account);

            var result = await controller.GetById(account.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAccount = Assert.IsType<Account>(okResult.Value);
            Assert.Equal(account.Id, returnedAccount.Id);
        }
    }
}