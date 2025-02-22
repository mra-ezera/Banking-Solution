using Banking.Controllers;
using Banking.Interfaces;
using Banking.Models.Entities;
using Banking.Models.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Banking.Tests.Controllers
{
    public class TransfersControllerTests
    {
        [Fact]
        public async Task Transfer_ValidTransfer_ReturnsOk()
        {
            var transferServiceMock = new Mock<ITransferService>();

            var fromAccount = new Account
            {
                Id = Guid.NewGuid(),
                Balance = 100,
                Name = "John",
                Surname = "Doe",
                Email = "john.doe@example.com"
            };

            var toAccount = new Account
            {
                Id = Guid.NewGuid(),
                Balance = 50,
                Name = "Jane",
                Surname = "Doe",
                Email = "jane.doe@example.com"
            };

            var transferDto = new TransferBalanceDto { ToAccountId = toAccount.Id, Amount = 50, Description = "Test transfer" };

            var updatedFromAccount = new Account
            {
                Id = fromAccount.Id,
                Balance = fromAccount.Balance - transferDto.Amount,
                Name = fromAccount.Name,
                Surname = fromAccount.Surname,
                Email = fromAccount.Email
            };

            var updatedToAccount = new Account
            {
                Id = toAccount.Id,
                Balance = toAccount.Balance + transferDto.Amount,
                Name = toAccount.Name,
                Surname = toAccount.Surname,
                Email = toAccount.Email
            };

            transferServiceMock.Setup(service => service.TransferAsync(fromAccount.Id, transferDto))
                .ReturnsAsync(Result<(Account FromAccount, Account ToAccount)>.Success((updatedFromAccount, updatedToAccount)));

            var controller = new TransfersController(transferServiceMock.Object);

            var result = await controller.Transfer(fromAccount.Id, transferDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TransferResult>(okResult.Value);

            Assert.Equal(50, returnValue.FromAccount.Balance);
            Assert.Equal(100, returnValue.ToAccount.Balance);
        }

        [Fact]
        public async Task Transfer_InsufficientFunds_ReturnsBadRequest()
        {
            var transferServiceMock = new Mock<ITransferService>();
            var fromAccount = new Account
            {
                Id = Guid.NewGuid(),
                Balance = 100,
                Name = "John",
                Surname = "Doe",
                Email = "john.doe@example.com"
            };
            var toAccount = new Account
            {
                Id = Guid.NewGuid(),
                Balance = 50,
                Name = "Jane",
                Surname = "Doe",
                Email = "jane.doe@example.com"
            };
            var transferDto = new TransferBalanceDto { ToAccountId = toAccount.Id, Amount = 150, Description = "Test transfer" };

            transferServiceMock.Setup(service => service.TransferAsync(fromAccount.Id, transferDto))
                .ReturnsAsync(Result<(Account FromAccount, Account ToAccount)>.Failure("Insufficient funds"));

            var controller = new TransfersController(transferServiceMock.Object);

            var result = await controller.Transfer(fromAccount.Id, transferDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
            Assert.Equal("Insufficient funds", errorResponse.Error);
        }

        [Fact]
        public async Task Transfer_ReturnsInternalServerError_OnException()
        {
            var transferServiceMock = new Mock<ITransferService>();
            var fromAccount = new Account
            {
                Id = Guid.NewGuid(),
                Balance = 100,
                Name = "John",
                Surname = "Doe",
                Email = "john.doe@example.com"
            };
            var transferDto = new TransferBalanceDto { ToAccountId = Guid.NewGuid(), Amount = 50, Description = "Test transfer" };

            transferServiceMock.Setup(service => service.TransferAsync(fromAccount.Id, transferDto))
                .ThrowsAsync(new Exception());

            var controller = new TransfersController(transferServiceMock.Object);

            var result = await controller.Transfer(fromAccount.Id, transferDto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var errorResponse = Assert.IsType<ErrorResponse>(objectResult.Value);
            Assert.Equal("An error occurred while processing your request", errorResponse.Error);
        }
    }
}
