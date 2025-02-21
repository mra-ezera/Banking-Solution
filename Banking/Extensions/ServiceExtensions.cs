using Banking.Data;
using Banking.Interfaces;
using Banking.Services;
using Microsoft.EntityFrameworkCore;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
            new MySqlServerVersion(new Version(8, 0, 23))));

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ITransferService, TransferService>();
        services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }
}
