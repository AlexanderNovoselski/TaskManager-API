using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data.Enums;
using TaskManager.Data.Models;

namespace TaskManager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Additional configurations
            modelBuilder.Entity<ToDoTask>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<ToDoTask>()
                .Property(t => t.AddedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()");

            // Seed initial data
            modelBuilder.Entity<ToDoTask>().HasData(
                new ToDoTask
                {
                    Id = Guid.NewGuid(),
                    Name = "Sample Task 1",
                    Description = "This is a sample task.",
                    DueDate = DateTime.UtcNow.AddDays(7),
                    UpdatedDate = DateTime.UtcNow
                },
                new ToDoTask
                {
                    Id = Guid.NewGuid(),
                    Name = "Sample Task 2",
                    Description = "Another sample task.",
                    ImportanceLevel = Importance.High,
                    DueDate = DateTime.UtcNow.AddDays(14),
                    UpdatedDate = DateTime.UtcNow
                }
            );
        }
    }
}
