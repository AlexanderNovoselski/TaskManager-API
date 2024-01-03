
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Data.Enums;
using TaskManager.Data.Models;
using TaskManager.Models.Requests;
using TaskManager.Services;
using TaskManager.Services.Contracts;

namespace TaskManager.MsUnitTesting;

[TestClass]
public class TaskServiceTests
{
    private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: "TestDatabase")
        .Options;

    private ApplicationDbContext context;
    private ITaskService taskService;

    [TestInitialize]
    public void Setup()
    {
        context = new ApplicationDbContext(dbContextOptions);
        context.Database.EnsureCreated();

        SeedDatabase();
        taskService = new TaskService(context);
    }

    [TestCleanup]
    public void TearDown()
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }

    private void SeedDatabase()
    {
        var tasks = new List<ToDoTask>()
        {
            new ToDoTask
            {
                Id = Guid.Parse("958bda36-754a-4358-b991-225d8de25e92"),
                OwnerId = "testOwnerId1",
                Name = "Task 1",
                Description = "Description for Task 1",
                ImportanceLevel = Importance.High,
                IsCompleted = false,
                DueDate = DateTime.UtcNow.AddDays(7)
            },
            new ToDoTask
            {
                Id = Guid.Parse("3c6eb404-869a-4da1-bd9f-4a88ac42a7e7"),
                OwnerId = "testOwnerId2",
                Name = "Task 2",
                Description = "Description for Task 2",
                ImportanceLevel = Importance.Low,
                IsCompleted = true,
                DueDate = DateTime.UtcNow.AddDays(14)
            },
            // Add more tasks as needed
        };

        context.Tasks.AddRange(tasks);
        context.SaveChanges();
    }

    [TestMethod]
    public async Task UpdateCompletition_ValidTask_UpdatesCompletionStatus()
    {
        // Arrange
        var request = new PatchTaskRequest { Id = Guid.Parse("958bda36-754a-4358-b991-225d8de25e92"), IsCompleted = true };
        var ownerId = "testOwnerId1";

        // Act
        await taskService.UpdateCompletition(request, ownerId);

        // Assert
        // Add your assertions here
    }

    // Add other test methods...
}
