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
        }

        public DbSet<User> Users { get; set; }

        public DbSet<FarmWorker> FarmWorker { get; set; }
    }
}
