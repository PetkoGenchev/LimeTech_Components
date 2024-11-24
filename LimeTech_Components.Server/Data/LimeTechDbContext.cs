﻿using LimeTech_Components.Server.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LimeTech_Components.Server.Data
{
    public class LimeTechDbContext : IdentityDbContext<User>
    {
        public LimeTechDbContext(DbContextOptions<LimeTechDbContext> options)
            : base(options)
        {
        }

        public DbSet<BuildCompatibility> BuildCompatibilities { get; init; }
        public DbSet<Component> Components { get; init; }
        public DbSet<User> Users { get; init; }
        public DbSet<DiscountMonth> DiscountMonths { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder
                .Entity<Component>()
                .HasOne(b => b.BuildCompatibility)
                .WithMany(c => c.Components)
                .HasForeignKey(b => b.Id)
                .OnDelete(DeleteBehavior.Restrict);

            //builder
            //    .Entity<Component>()
            //    .HasOne(u => u.User)
            //    .WithMany(c => c.ComponentBasket)
            //    .HasForeignKey(b => b.Id)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Component>()
                .HasOne(u => u.DiscountMonth)
                .WithMany(c => c.Components)
                .HasForeignKey(b => b.Id)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);
        }

    }
}
