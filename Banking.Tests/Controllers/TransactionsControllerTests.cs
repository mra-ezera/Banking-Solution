using Banking.Controllers;
using Banking.Interfaces;
using Banking.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System;
using System.Threading.Tasks;
using Banking.Models.Results;
using Banking.Models.DTOs;

namespace Banking.Tests.Controllers
{
    public class TransactionsControllerTests
    {
        [Fact]
        public async Task AddToBalance_ValidData_ReturnsOkResult()
        {
            var mockService = new Mock<ITransactionService>();
            var controller = new TransactionsController(mockService.Object);
            var updateBalanceDto = new UpdateBalanceDto { Amount = 100 };
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Balance = 200,
                Name = "John",
                Surname = "Doe",
                Email = "john.doe@example.com"
            };

            mockService
                .Setup(s => s.AddBalanceAsync(It.IsAny<Guid>(), It.IsAny<UpdateBalanceDto>()))
                .ReturnsAsync(Result<Account>.Success(account));

            var result = await controller.AddToBalance(account.Id, updateBalanceDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAccount = Assert.IsType<Account>(okResult.Value);
            Assert.Equal(account.Id, returnedAccount.Id);
        }

        [Fact]
        public async Task RemoveFromBalance_ValidData_ReturnsOkResult()
        {
            var mockService = new Mock<ITransactionService>();
            var controller = new TransactionsController(mockService.Object);
            var updateBalanceDto = new UpdateBalanceDto { Amount = 50 };
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Balance = 150,
                Name = "John",
                Surname = "Doe",
                Email = "john.doe@example.com"
            };

            mockService
                .Setup(s => s.RemoveBalanceAsync(It.IsAny<Guid>(), It.IsAny<UpdateBalanceDto>()))
                .ReturnsAsync(Result<Account>.Success(account));

            var result = await controller.RemoveFromBalance(account.Id, updateBalanceDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAccount = Assert.IsType<Account>(okResult.Value);
            Assert.Equal(account.Id, returnedAccount.Id);
        }
    }
}

