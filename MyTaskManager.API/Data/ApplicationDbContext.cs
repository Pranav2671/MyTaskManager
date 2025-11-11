using Microsoft.EntityFrameworkCore;
using MyTaskManager.Shared.Models;
using System;

namespace MyTaskManager.API.Data
{
    // Represents a single user in the system (this becomes the "Users" table in the DB)
    public class User
    {
        // Unique user ID (e.g., a GUID stored as a string)
        public required string Id { get; set; }

        // Username chosen by the user (must be unique)
        public required string Username { get; set; }

        // Hashed password (never store plain passwords!)
        public required string PasswordHash { get; set; }
    }

    // This class is the main bridge between the application and the database.
    // It defines all the tables and their relationships using Entity Framework Core.
    public class ApplicationDbContext : DbContext
    {
        // Constructor – receives database connection options (set in Program.cs or Startup.cs)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets represent tables in the database.
        // Each DbSet<T> corresponds to one table.
        public DbSet<TaskItem> TaskItems { get; set; }   // Table for quiz tasks or questions
        public DbSet<User> Users { get; set; }           // Table for registered users

        // This method is used to configure how tables and columns are created.
        // It’s called automatically when EF Core builds the database model.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === TaskItem Table Configuration ===
            modelBuilder.Entity<TaskItem>(b =>
            {
                // Primary key (unique identifier for each record)
                b.HasKey(t => t.Id);

                // Make Title required and limit its length to 200 characters
                b.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                // Set default value for CreatedAt to the current UTC time (handled by the DB)
                b.Property(t => t.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Store TaskStatus enum as integer in the database (default EF behavior)
                b.Property(t => t.Status)
                    .HasConversion<int>()
                    .IsRequired();
            });

            // === User Table Configuration ===
            modelBuilder.Entity<User>(b =>
            {
                // Primary key (unique ID for each user)
                b.HasKey(u => u.Id);

                // Username is required and max 100 characters
                b.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(100);

                // Add a unique index to Username to prevent duplicate accounts
                b.HasIndex(u => u.Username).IsUnique();
            });
        }
    }
}
