using Banking.Interfaces;
using Banking.Models.DTOs;
using Banking.Models.Entities;
using Banking.Services;
using Banking.Tests.Helpers;
using Moq;

namespace Banking.Tests.Services
{
    public class TransactionServiceTests
    {
        [Fact]
        public async Task AddBalanceAsync_ValidAmount_ReturnsUpdatedBalance()
        {
            using var context = DatabaseHelper.CreateDbContext();
            var accountService = new AccountService(context);
            var historyServiceMock = new Mock<ITransactionHistoryService>();
            var service = new TransactionService(accountService, historyServiceMock.Object);

            var account = new Account
            {
                Name = "John",
                Surname = "Doe",
                Email = "john.doe@example.com",
                Balance = 100,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };
            context.Accounts.Add(account);
            await context.SaveChangesAsync();

            var dto = new UpdateBalanceDto
            {
                Amount = 50,
                Description = "Test deposit"
            };

            var result = await service.AddBalanceAsync(account.Id, dto);

            Assert.True(result.IsSuccess);
            Assert.Equal(150, result.Data.Balance);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(150, result.Data.Balance);
        }

        [Fact]
        public async Task RemoveBalanceAsync_InsufficientFunds_ReturnsFailure()
        {
            using var context = DatabaseHelper.CreateDbContext();
            var accountService = new AccountService(context);
            var historyServiceMock = new Mock<ITransactionHistoryService>();
            var service = new TransactionService(accountService, historyServiceMock.Object);

            var account = new Account
            {
                Name = "John",
                Surname = "Doe",
                Email = "john.doe@example.com",
                Balance = 100,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };
            context.Accounts.Add(account);
            await context.SaveChangesAsync();

            var dto = new UpdateBalanceDto
            {
                Amount = 150,
                Description = "Test withdrawal"
            };

            var result = await service.RemoveBalanceAsync(account.Id, dto);

            Assert.False(result.IsSuccess);
            Assert.Contains("Insufficient funds", result.Error);
        }
    }
}
