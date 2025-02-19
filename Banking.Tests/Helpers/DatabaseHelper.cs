using Banking.Data;
using Microsoft.EntityFrameworkCore;

namespace Banking.Tests.Helpers
{
    public static class DatabaseHelper
    {
        public static ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
