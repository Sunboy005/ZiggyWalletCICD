using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Data.EFCore
{
    public class ZiggyDBContext : IdentityDbContext
    {
        public ZiggyDBContext(DbContextOptions<ZiggyDBContext> options) : base(options)
        {

        }


        public DbSet<WalletToReturn> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        //public DbSet<TranxType> TranxTypes { get; set; }
        //public DbSet<Status> Statuses { get; set; }
        public DbSet<WalletCurrency> WalletCurrency { get; set; }


        //protected override onModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<AppUser>()
        //        .HasMany(x => x.Wallets)
        //        .WithOne(x => x.AppUser);
        //}
    }
}
