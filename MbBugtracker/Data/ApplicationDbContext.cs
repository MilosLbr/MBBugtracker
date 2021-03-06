﻿using System;
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
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectsAndUsers> ProjectsAndUsers { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<TicketResolution> TicketResolution { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketActivityLog> TicketActivityLogs { get; set; }
        public DbSet<ProjectStatus> ProjectStatuses { get; set; }

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

            modelBuilder.Entity<TicketStatus>(b =>
            {
                b.HasData(new TicketStatus { Id = 1, StatusName = "Open" },
                    new TicketStatus { Id = 2, StatusName = "Closed" },
                    new TicketStatus { Id = 3, StatusName = "In progress" },
                    new TicketStatus { Id = 4, StatusName = "To be tested" },
                    new TicketStatus { Id = 5, StatusName = "Reopen" });
            });

            modelBuilder.Entity<TicketType>(b =>
            {
                b.HasData(new TicketType { Id = 1, TypeName = "Code bug" },
                    new TicketType { Id = 2, TypeName = "Crash/Hang" },
                    new TicketType { Id = 3, TypeName = "Data loss" },
                    new TicketType { Id = 4, TypeName = "Performance" },
                    new TicketType { Id = 5, TypeName = "UI" },
                    new TicketType { Id = 6, TypeName = "New Feature" },
                    new TicketType { Id = 7, TypeName = "Enhancement" },
                    new TicketType { Id = 8, TypeName = "Security" },
                    new TicketType { Id = 9, TypeName = "Other" });
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
