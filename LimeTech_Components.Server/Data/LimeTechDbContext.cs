using LimeTech_Components.Server.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder
                .Entity<Component>()
                .HasOne(b => b.BuildCompatibility)
                .WithMany(c => c.Components)
                .HasForeignKey(b => b.BuildId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Component>()
                .HasOne(b => b.PurchaseHistory)
                .WithMany(c => c.Components)
                .HasForeignKey(b => b.PurchaseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BasketItem>()
                .HasOne(b => b.Component)
                .WithMany(i => i.BasketItems)
                .HasForeignKey(b => b.ComponentID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BasketItem>()
                .HasOne(b => b.Customer)
                .WithMany(i => i.BasketItems)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PurchaseHistory>()
                .HasOne(b => b.Customer)
                .WithMany(i => i.PurchaseHistories)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);




            base.OnModelCreating(builder);
        }

    }
}
