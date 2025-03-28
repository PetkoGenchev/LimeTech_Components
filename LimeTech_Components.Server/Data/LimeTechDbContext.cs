using LimeTech_Components.Server.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LimeTech_Components.Server.Data
{
    public class LimeTechDbContext : IdentityDbContext<Customer>
    {
        public LimeTechDbContext(DbContextOptions<LimeTechDbContext> options)
            : base(options)
        {
        }

        public DbSet<BuildCompatibility> BuildCompatibilities { get; init; }
        public DbSet<Component> Components { get; init; }
        public DbSet<Customer> Customers { get; init; }
        public DbSet<BasketItem> BasketItems { get; init; }
        public DbSet<PurchaseHistory> PurchaseHistories { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<Component>()
                .HasOne(c => c.BuildCompatibility)
                .WithMany(b => b.Components)
                .HasForeignKey(c => c.BuildId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PurchaseHistory>()
                .HasOne(ph => ph.Component)
                .WithMany(c => c.PurchaseHistories)
                .HasForeignKey(ph => ph.ComponentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PurchaseHistory>()
                .HasOne(ph => ph.Customer)
                .WithMany(c => c.PurchaseHistories)
                .HasForeignKey(ph => ph.CustomerId)
                .HasPrincipalKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BasketItem>()
                .HasOne(bi => bi.Customer)
                .WithMany(c => c.BasketItems)
                .HasForeignKey(bi => bi.CustomerId)
                .HasPrincipalKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BasketItem>()
                .HasOne(bi => bi.Customer)
                .WithMany(c => c.BasketItems)
                .HasForeignKey(bi => bi.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PurchaseHistory>()
                .Property(p => p.TotalPrice)
                .HasPrecision(18, 2);



        }

    }
}
