using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortalAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AcademyPortalAPI.Models
{
    public class AcademyPortalDbContext:IdentityDbContext<ApplicationUser, ApplicationRole, string,
        IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public AcademyPortalDbContext(DbContextOptions<AcademyPortalDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Batch>().HasOne(b => b.CreatedBy).WithMany(u => u.CreatedBatches);

            modelBuilder.Entity<BatchUser>()
                .HasKey(bu => new { bu.BatchId , bu.UserId});

            modelBuilder.Entity<BatchUser>()
                .HasOne(bu => bu.Batch)
                .WithMany(b => b.Users)
                .HasForeignKey(bu => bu.BatchId);

            modelBuilder.Entity<BatchUser>()
                .HasOne(bu => bu.User)
                .WithMany(bu => bu.Batches)
                .HasForeignKey(bu => bu.UserId);

            modelBuilder.Entity<ApplicationUser>(u =>
            {
                u.HasMany(e => e.UserRoles)
                .WithOne(e => e.ApplicationUser)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });

            modelBuilder.Entity<ApplicationRole>(r =>
            {
                r.HasMany(e => e.UserRoles)
                .WithOne(e => e.ApplicationRole)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            });    
        }
        
        public DbSet<Status> AllStatus { get; set; } 
        public DbSet<Skill> Skills { get; set; } 
        public DbSet<Module> Modules { get; set; } 
        public DbSet<Batch> Batches { get; set; }
        public DbSet<BatchUser> BatchUser { get; set; }
    }
}
