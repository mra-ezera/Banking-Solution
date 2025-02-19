using Banking.Interfaces;
using Banking.Models.Entities;
using Banking.Services;
using Banking.Tests.Helpers;
using Moq;

namespace Banking.Tests.Services
{
    public class TransferServiceTests
    {
        [Fact]
        public async Task TransferAsync_ValidTransfer_UpdatesBalances()
        {
            using var context = DatabaseHelper.CreateDbContext();
            var accountService = new AccountService(context);
            var historyServiceMock = new Mock<ITransactionHistoryService>();
            var service = new TransferService(accountService, historyServiceMock.Object);

            var fromAccount = new Account
            {
                Name = "John",
                Surname = "Doe",
                Email = "john@example.com",
                Balance = 100,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            var toAccount = new Account
            {
                Name = "Jane",
                Surname = "Doe",
                Email = "jane@example.com",
                Balance = 50,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            context.Accounts.AddRange(fromAccount, toAccount);
            await context.SaveChangesAsync();

            var transferDto = new TransferBalanceDto
            {
                ToAccountId = toAccount.Id,
                Amount = 50,
                Description = "Test transfer"
            };

            var result = await service.TransferAsync(fromAccount.Id, transferDto);

            Assert.True(result.IsSuccess);
            Assert.Equal(50, result.Data.FromAccount.Balance);
            Assert.Equal(100, result.Data.ToAccount.Balance);
        }

        [Fact]
        public async Task TransferAsync_InsufficientFunds_ReturnsFailure()
        {
            using var context = DatabaseHelper.CreateDbContext();
            var accountService = new AccountService(context);
            var historyServiceMock = new Mock<ITransactionHistoryService>();
            var service = new TransferService(accountService, historyServiceMock.Object);

            var fromAccount = new Account
            {
                Name = "John",
                Surname = "Doe",
                Email = "john@example.com",
                Balance = 100,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            var toAccount = new Account
            {
                Name = "Jane",
                Surname = "Doe",
                Email = "jane@example.com",
                Balance = 50,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            context.Accounts.AddRange(fromAccount, toAccount);
            await context.SaveChangesAsync();

            var transferDto = new TransferBalanceDto
            {
                ToAccountId = toAccount.Id,
                Amount = 150,
                Description = "Test transfer"
            };

            var result = await service.TransferAsync(fromAccount.Id, transferDto);

            Assert.False(result.IsSuccess);
            Assert.Contains("Insufficient funds", result.Error);
        }
    }
}