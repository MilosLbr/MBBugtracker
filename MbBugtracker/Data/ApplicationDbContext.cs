using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MbBugtracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        // Ctor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Properties
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectsAndUsers> ProjectsAndUsers { get; set; }


        // Override onmodel creating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne()
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            modelBuilder.Entity<Ticket>(b =>
            {
                b.HasOne(t => t.ApplicationUser)
                    .WithMany(au => au.Tickets)
                    .HasForeignKey(t => t.ApplicationUserId)
                    .IsRequired();
            });

            modelBuilder.Entity<ProjectsAndUsers>()
                .HasKey(pau => new { pau.ProjectId, pau.ApplicationUserId });
            modelBuilder.Entity<ProjectsAndUsers>()
                .HasOne(pau => pau.Project)
                .WithMany(p => p.ProjectsAndUsers)
                .HasForeignKey(pau => pau.ProjectId);
            modelBuilder.Entity<ProjectsAndUsers>()
                .HasOne(pau => pau.ApplicationUser)
                .WithMany(au => au.ProjectsAndUsers)
                .HasForeignKey(pau => pau.ApplicationUserId);

        }
    }
}
