using Microsoft.EntityFrameworkCore;
using MyTaskManager.Shared.Models;

namespace MyTaskManager.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TaskItem Configuration
            modelBuilder.Entity<TaskItem>(b =>
            {
                b.HasKey(t => t.Id);

                b.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                b.Property(t => t.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                b.Property(t => t.Status)
                    .HasConversion<int>()
                    .IsRequired();
            });

            // User Configuration
            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);

                b.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(100);

                b.HasIndex(u => u.Username).IsUnique();
            });
        }
    }
}
