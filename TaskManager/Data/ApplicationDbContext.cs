using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data.Enums;
using TaskManager.Data.Models;

namespace TaskManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ToDoTask>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<ToDoTask>()
                .Property(t => t.AddedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()");

            // Define the relationship between ToDoTask and IdentityUser
            modelBuilder.Entity<ToDoTask>()
                .HasOne(t => t.Owner)
                .WithMany()
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial data

            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "1",
                    UserName = "user1@example.com",
                    NormalizedUserName = "USER1@EXAMPLE.COM",
                    Email = "user1@example.com",
                    NormalizedEmail = "USER1@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "Password123"),
                    SecurityStamp = string.Empty,
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                });

            modelBuilder.Entity<ToDoTask>().HasData(
                new ToDoTask
                {
                    Id = Guid.NewGuid(),
                    OwnerId = "1",
                    Name = "Sample Task 1",
                    Description = "This is a sample task.",
                    DueDate = DateTime.UtcNow.AddDays(7),
                    UpdatedDate = DateTime.UtcNow
                },
                new ToDoTask
                {
                    Id = Guid.NewGuid(),
                    OwnerId = "1",
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
