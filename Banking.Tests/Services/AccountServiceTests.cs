using Banking.Models.DTOs;
using Banking.Models.Entities;
using Banking.Services;
using Banking.Tests.Helpers;

namespace Banking.Tests.Services
{
    public class AccountServiceTests
    {
        [Fact]
        public async Task CreateAccountAsync_ValidData_ReturnsSuccess()
        {
            using var context = DatabaseHelper.CreateDbContext();
            var service = new AccountService(context);
            var dto = new AddAccountDto
            {
                Name = "John",
                Surname = "Doe",
                Email = "john.doe@example.com"
            };

            var result = await service.CreateAccountAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(dto.Name, result.Data.Name);
            Assert.Equal(0, result.Data.Balance);
        }

        [Fact]
        public async Task GetAccountByIdAsync_ExistingAccount_ReturnsAccount()
        {
            using var context = DatabaseHelper.CreateDbContext();
            var service = new AccountService(context);
            var account = new Account
            {
                Name = "John",
                Surname = "Doe",
                Email = "john.doe@example.com",
                Balance = 0,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };
            context.Accounts.Add(account);
            await context.SaveChangesAsync();

            var result = await service.GetAccountByIdAsync(account.Id);

            Assert.NotNull(result);
            Assert.Equal(account.Id, result.Id);
        }
    }
}
