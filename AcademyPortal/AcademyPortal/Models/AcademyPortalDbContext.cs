using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AcademyPortal.Models
{
    public class AcademyPortalDbContext:IdentityDbContext<ApplicationUser>
    {
        public AcademyPortalDbContext(DbContextOptions<AcademyPortalDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelbBuilder)
        {
            base.OnModelCreating(modelbBuilder);
            modelbBuilder.Entity<Batch>().HasOne(b => b.CreatedBy).WithMany(u => u.CreatedBatches);

            modelbBuilder.Entity<BatchUser>()
                .HasKey(bu => new { bu.BatchId , bu.UserId});

            modelbBuilder.Entity<BatchUser>()
                .HasOne(bu => bu.Batch)
                .WithMany(b => b.Users)
                .HasForeignKey(bu => bu.BatchId);

            modelbBuilder.Entity<BatchUser>()
                .HasOne(bu => bu.User)
                .WithMany(bu => bu.Batches)
                .HasForeignKey(bu => bu.UserId);
        }
        
        public DbSet<Status> AllStatus { get; set; } 
        public DbSet<Skill> Skills { get; set; } 
        public DbSet<Module> Modules { get; set; } 
        public DbSet<Batch> Batches { get; set; }
    }
}
