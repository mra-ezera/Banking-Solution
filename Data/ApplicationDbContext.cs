using Microsoft.EntityFrameworkCore;
using Banking.Models.Entities;

namespace Banking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountHistory> AccountHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Account>()
                .Property(a => a.Id)
                .HasColumnType("char(36)");

            modelBuilder.Entity<Account>()
                .Property(a => a.DateCreated)
                .HasColumnType("datetime");

            modelBuilder.Entity<Account>()
                .Property(a => a.DateModified)
                .HasColumnType("datetime");

            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<AccountHistory>()
                .HasKey(ah => ah.Id);

            modelBuilder.Entity<AccountHistory>()
                .Property(ah => ah.AccountId)
                .HasColumnType("char(36)");

            modelBuilder.Entity<AccountHistory>()
                .Property(ah => ah.TransactionDate)
                .HasColumnType("datetime");

            modelBuilder.Entity<AccountHistory>()
                .Property(ah => ah.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<AccountHistory>()
                .HasOne(ah => ah.Account)
                .WithMany(a => a.AccountHistories)
                .HasForeignKey(ah => ah.AccountId);
        }
    }
}
