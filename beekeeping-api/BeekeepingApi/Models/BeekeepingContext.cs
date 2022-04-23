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

            modelBuilder.Entity<BeeFamily>()
                .HasMany<ApiaryBeeFamily>(b => b.ApiaryBeeFamilies)
                .WithOne(ab => ab.BeeFamily)
                .HasForeignKey(ab => ab.BeeFamilyId)
                .OnDelete(DeleteBehavior.ClientCascade);
            
            modelBuilder.Entity<Apiary>()
                .HasMany<Harvest>(a => a.Harvests)
                .WithOne(ab => ab.Apiary)
                .HasForeignKey(ab => ab.ApiaryId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Food>()
                .HasMany<Feeding>(fo => fo.Feedings)
                .WithOne(fe => fe.Food)
                .HasForeignKey(fe => fe.FoodId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Beehive>()
                .HasMany<BeehiveBeeFamily>(b => b.BeehiveBeeFamilies)
                .WithOne(bbf => bbf.Beehive)
                .HasForeignKey(bbf => bbf.BeehiveId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Queen>()
                .HasMany<BeefamilyQueen>(q => q.BeeFamilyQueens)
                .WithOne(bq => bq.Queen)
                .HasForeignKey(bq => bq.QueenId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Queen>()
                .HasMany<QueensRaising>(q => q.QueensRaisings)
                .WithOne(qr => qr.Mother)
                .HasForeignKey(qr => qr.MotherId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<FarmWorker> FarmWorkers { get; set; }

        public DbSet<Farm> Farms { get; set; }

        public DbSet<Apiary> Apiaries { get; set; }

        public DbSet<BeeFamily> BeeFamilies { get; set; }

        public DbSet<ApiaryBeeFamily> ApiaryBeeFamilies { get; set; }

        public DbSet<Beehive> Beehives { get; set; }

        public DbSet<BeehiveBeeFamily> BeehiveBeeFamilies { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<BeehiveComponent> BeehiveComponents { get; set; }

        public DbSet<Harvest> Harvests { get; set; }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<NestReduction> NestReductions { get; set; }

        public DbSet<NestExpansion> NestExpansions { get; set; }

        public DbSet<Food> Foods { get; set; }

        public DbSet<Feeding> Feedings { get; set; }

        public DbSet<Queen> Queens { get; set; }

        public DbSet<BeefamilyQueen> BeeFamilyQueens { get; set; }

        public DbSet<QueensRaising> QueensRaisings { get; set; }
        
        public DbSet<Invitation> Invitations { get; set; }
    }
}
