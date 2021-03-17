using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeekeepingApi.Models;

namespace BeekeepingApi.Models
{
    public class BeekeepingContext : DbContext
    {
        public BeekeepingContext(DbContextOptions<BeekeepingContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FarmWorker>().HasKey(e => new { e.UserId, e.FarmId });

            modelBuilder.Entity<Apiary>()
                .HasMany<ApiaryBeehive>(a => a.ApiaryBeehives)
                .WithOne(ab => ab.Apiary)
                .HasForeignKey(ab => ab.ApiaryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Beehive>()
                .HasMany<ApiaryBeehive>(b => b.ApiaryBeehives)
                .WithOne(ab => ab.Beehive)
                .HasForeignKey(ab => ab.BeehiveId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<FarmWorker> FarmWorkers { get; set; }

        public DbSet<Farm> Farms { get; set; }

        public DbSet<Apiary> Apiaries { get; set; }

        public DbSet<Beehive> Beehives { get; set; }

        public DbSet<ApiaryBeehive> ApiaryBeehives { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<Super> Supers { get; set; }

        public DbSet<BeekeepingApi.Models.Harvest> Harvest { get; set; }
    }
}
