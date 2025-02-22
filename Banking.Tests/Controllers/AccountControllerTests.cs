using Banking.Controllers;
using Banking.Interfaces;
using Banking.Models.DTOs;
using Banking.Models.Entities;
using Banking.Models.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Banking.Tests.Controllers
{
    public class AccountControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfAccounts()
        {
            var mockService = new Mock<IAccountService>();
            var accounts = new List<Account>
    {
        new Account { Id = Guid.NewGuid(), Name = "John", Surname = "Doe", Email = "john.doe@example.com" },
        new Account { Id = Guid.NewGuid(), Name = "Jane", Surname = "Doe", Email = "jane.doe@example.com" }
    };
            var paginationParams = new PaginationParamsDto { PageNumber = 1, PageSize = 10 };
            mockService.Setup(s => s.GetAllAccountsAsync(paginationParams)).ReturnsAsync(new PagedResultDto<Account> { Items = accounts, TotalCount = accounts.Count });
            var controller = new AccountController(mockService.Object);

            var result = await controller.GetAll(paginationParams);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPagedResult = Assert.IsType<PagedResultDto<Account>>(okResult.Value);
            Assert.Equal(2, returnedPagedResult.Items.Count());
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithEmptyList()
        {
            var mockService = new Mock<IAccountService>();
            var accounts = new List<Account>();
            var paginationParams = new PaginationParamsDto { PageNumber = 1, PageSize = 10 };
            mockService.Setup(s => s.GetAllAccountsAsync(paginationParams)).ReturnsAsync(new PagedResultDto<Account> { Items = accounts, TotalCount = accounts.Count });
            var controller = new AccountController(mockService.Object);

            var result = await controller.GetAll(paginationParams);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPagedResult = Assert.IsType<PagedResultDto<Account>>(okResult.Value);
            Assert.Empty(returnedPagedResult.Items);
        }

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
                Email = dto.Email,
                Balance = 0,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                AccountHistories = new List<AccountHistory>()
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
                Email = "john.doe@example.com",
                Balance = 0,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                AccountHistories = new List<AccountHistory>()
            };

            mockService
                .Setup(s => s.GetAccountByIdAsync(account.Id))
                .ReturnsAsync(account);

            var result = await controller.GetById(account.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAccount = Assert.IsType<Account>(okResult.Value);
            Assert.Equal(account.Id, returnedAccount.Id);
        }

        [Fact]
        public async Task GetAll_ReturnsInternalServerError_OnException()
        {
            var mockService = new Mock<IAccountService>();
            var paginationParams = new PaginationParamsDto { PageNumber = 1, PageSize = 10 };
            mockService.Setup(service => service.GetAllAccountsAsync(paginationParams))
                .ThrowsAsync(new Exception());
            var controller = new AccountController(mockService.Object);

            var result = await controller.GetAll(paginationParams);

            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            var errorResponse = Assert.IsType<ErrorResponse>(objectResult.Value);
            Assert.Equal("An error occurred while processing your request", errorResponse.Error);
        }

        [Fact]
        public async Task GetById_ReturnsInternalServerError_OnException()
        {
            var mockService = new Mock<IAccountService>();
            var accountId = Guid.NewGuid();
            mockService.Setup(service => service.GetAccountByIdAsync(accountId))
                .ThrowsAsync(new Exception());
            var controller = new AccountController(mockService.Object);

            var result = await controller.GetById(accountId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            var errorResponse = Assert.IsType<ErrorResponse>(objectResult.Value);
            Assert.Equal("An error occurred while processing your request", errorResponse.Error);
        }

        [Fact]
        public async Task Create_ReturnsInternalServerError_OnException()
        {
            var mockService = new Mock<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var addAccountDto = new AddAccountDto { Name = "John", Surname = "Doe", Email = "john.doe@example.com", Balance = 1000 };
            mockService.Setup(service => service.CreateAccountAsync(addAccountDto))
                .ThrowsAsync(new Exception());

            var result = await controller.Create(addAccountDto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var errorResponse = Assert.IsType<ErrorResponse>(objectResult.Value);
            Assert.Equal("An error occurred while processing your request", errorResponse.Error);
        }

    }
}

