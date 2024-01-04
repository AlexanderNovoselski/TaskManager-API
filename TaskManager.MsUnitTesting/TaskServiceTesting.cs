
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TaskManager.Data;
using TaskManager.Data.Enums;
using TaskManager.Data.Models;
using TaskManager.Models;
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
            new ToDoTask
            {
                Id = Guid.Parse("0a44cf2f-0479-4a74-9c30-9d5f2a05ef0a"),
                OwnerId = "testOwnerId2",
                Name = "Task for testing",
                Description = "Description for Task 2",
                ImportanceLevel = Importance.Low,
                IsCompleted = true,
                DueDate = DateTime.UtcNow.AddDays(14)
            },
        };

        context.Tasks.AddRange(tasks);
        context.SaveChanges();
    }

    [TestMethod]
    public async Task UpdateCompletition_ValidTask_UpdatesCompletionStatusToTrue()
    {
        // Arrange
        var request = new PatchTaskRequest { Id = Guid.Parse("958bda36-754a-4358-b991-225d8de25e92"), IsCompleted = true };
        var request2 = new PatchTaskRequest { Id = Guid.Parse("3c6eb404-869a-4da1-bd9f-4a88ac42a7e7"), IsCompleted = false };
        var ownerId = "testOwnerId1";
        var ownerId2 = "testOwnerId2";

        // Act
        await taskService.UpdateCompletition(request, ownerId);
        await taskService.UpdateCompletition(request2, ownerId2);

        // Assert
        var updatedTask = await taskService.GetById(request.Id, ownerId);
        var updatedTask2 = await taskService.GetById(request2.Id, ownerId2);

        Assert.IsNotNull(updatedTask, "Task not found in the database");
        Assert.IsTrue(updatedTask.IsCompleted, "Task completion status not updated to true");

        Assert.IsNotNull(updatedTask2, "Task not found in the database");
        Assert.IsFalse(updatedTask2.IsCompleted, "Task completion status not updated to false");
    }

    [TestMethod]
    public async Task UpdateCompletition_ValidTask_UpdatesCompletionStatusToFalse()
    {
        // Arrange
        var request = new PatchTaskRequest { Id = Guid.Parse("958bda36-754a-4358-b991-225d8de25e92"), IsCompleted = false };
        var ownerId = "testOwnerId1";

        // Act
        await taskService.UpdateCompletition(request, ownerId);

        // Assert
        var updatedTask = await taskService.GetById(request.Id, ownerId);

        Assert.IsNotNull(updatedTask, "Task not found in the database");
        Assert.IsFalse(updatedTask.IsCompleted, "Task completion status not updated to false");

    }

    [TestMethod]
    public async Task UpdateCompletition_InvalidTask_UpdatesCompletionStatusTo()
    {
        // Arrange
        var request = new PatchTaskRequest { Id = Guid.Parse("BECF18A9-6BA8-431D-AF65-299537010172"), IsCompleted = false };
        var ownerId = "testOwnerId1";

        // Act and Assert
        await Assert.ThrowsExceptionAsync<TaskManagerException>(async () =>
        {
            await taskService.UpdateCompletition(request, ownerId);
        });
    }

    public async Task Create_SuccessfulTaskCreation()
    {
        // Arrange
        var ownerId = "testOwnerId1";
        var request = new TaskForCreationRequest
        {
            Name = "Test Task",
            Description = "Test Description",
            ImportanceLevel = "High",
            DueDate = DateTime.Now.AddDays(7)
        };

        // Act
        await taskService.Create(request, ownerId);

        // Assert
        var createdTask = await taskService.GetTasksPaginated(ownerId, 1, 1);

        Assert.IsNotNull(createdTask, "Created task should not be null");
        Assert.AreEqual(3, createdTask.Count(), "There should be exactly one task");

        var retrievedTask = createdTask.First();
        Assert.AreEqual(request.Name, retrievedTask.Name, "Name should match");
        Assert.AreEqual(request.Description, retrievedTask.Description, "Description should match");
        Assert.AreEqual(request.ImportanceLevel, retrievedTask.ImportanceLevel, "ImportanceLevel should match");
        Assert.AreEqual(request.DueDate, retrievedTask.DueDate, "DueDate should match");
    }

    [TestMethod]
    public async Task Create_ErrorDuringTaskCreation()
    {
        // Arrange
        var ownerId = "testOwnerId1";
        var request = new TaskForCreationRequest
        {
            Name = null,
            Description = null,
            ImportanceLevel = "InvalidImportance",
            DueDate = DateTime.Now.AddDays(-1)
        };

        // Act and Assert
        await Assert.ThrowsExceptionAsync<TaskManagerException>(async () =>
        {
            await taskService.Create(request, ownerId);
        });
    }

    [TestMethod]
    public async Task Create_PastDueDate_ThrowsValidationException()
    {
        // Arrange
        var ownerId = "testOwnerId1";
        var request = new TaskForCreationRequest
        {
            Name = "Test Task",
            Description = "Test Description",
            ImportanceLevel = "High",
            DueDate = DateTime.Now.AddDays(-1)
        };

        // Act and Assert
        try
        {
            await taskService.Create(request, ownerId);
            Assert.Fail("Expected TaskManagerException, but no exception was thrown.");
        }
        catch (TaskManagerException ex)
        {
            // Check if the inner exception is a ValidationException
            Assert.IsInstanceOfType(ex.InnerException, typeof(ValidationException));
        }
        catch (Exception)
        {
            Assert.Fail("Expected TaskManagerException, but a different exception was thrown.");
        }
    }

    [TestMethod]
    public async Task Delete_SuccessfulTaskRemoval()
    {
        // Arrange
        // Valid data
        var Id = Guid.Parse("958bda36-754a-4358-b991-225d8de25e92");
        string ownerId = "testOwnerId1";


        // Act
        await taskService.DeleteById(Id, ownerId);

        // Assert

        await Assert.ThrowsExceptionAsync<TaskManagerException>(async () =>
        {
            var deletedTask = await taskService.GetById(Id, ownerId);
        });
    }

    [TestMethod]
    public async Task Delete_InvalidTaskRemoval()
    {
        // Arrange
        // Invalid data
        var invalidId = Guid.Parse("BECF18A9-6BA8-431D-AF65-299537010172");
        string ownerId = "testOwnerId1";

        // Act and Assert
        await Assert.ThrowsExceptionAsync<TaskManagerException>(async () =>
        {
            var deletedTask = await taskService.GetById(invalidId, ownerId);
        });
    }

    [TestMethod]
    public async Task GetTasksPaginated_ReturnsCorrectNumberOfTasks()
    {
        // Arrange
        string ownerId = "testOwnerId1";
        int pageNumber = 1;
        int pageSize = 1; // Assuming you have seeded 2 tasks

        // Act
        var paginatedTasks = await taskService.GetTasksPaginated(ownerId, pageNumber, pageSize);

        // Assert
        Assert.IsNotNull(paginatedTasks, "Returned tasks should not be null");
        Assert.AreEqual(pageSize, paginatedTasks.Count(), $"Expected {pageSize} tasks for page {pageNumber}");

    }

    [TestMethod]
    public async Task GetTasksPaginated_InvalidOwnerIdReturnsZeroTasks()
    {
        // Arrange
        string invalidOwnerId = "nonexistentOwnerId";
        int pageNumber = 1;
        int pageSize = 0;

        // Act
        var paginatedTasks = await taskService.GetTasksPaginated(invalidOwnerId, pageNumber, pageSize);

        // Assert
        Assert.AreEqual(pageSize, paginatedTasks.Count(), $"Expected {pageSize} tasks for page {pageNumber}");
    }

    [TestMethod]
    public async Task GetTaskBy_WithValidDataIdReturnsTask()
    {
        // Arrange
        var validId = Guid.Parse("958bda36-754a-4358-b991-225d8de25e92");
        var validOwnerId = "testOwnerId1";

        // Act
        var task = await taskService.GetById(validId, validOwnerId);

        // Assert
        Assert.IsNotNull(task, "Task shouild not be null");
    }

    [TestMethod]
    public async Task GetTaskBy_WithInvalidDataIdReturnsTask()
    {
        // Arrange
        var validId = Guid.Parse("BECF18A9-6BA8-431D-AF65-299537010172");
        var validOwnerId = "testOwnerId12";

        // Act

        // Act and Assert
        await Assert.ThrowsExceptionAsync<TaskManagerException>(async () =>
        {
            var task = await taskService.GetById(validId, validOwnerId);
        });
    }

    [TestMethod]
    public async Task GetTasksCount_WithValidDataReturnsCount()
    {
        // Arrange
        var validOwnerId = "testOwnerId1";
        int expectedCount = 1;

        // Act
        int taskCount = await taskService.GetCountOfAll(validOwnerId);

        // Assert
        Assert.AreEqual(expectedCount, taskCount);
    }

    [TestMethod]
    public async Task GetTasksCount_WithInvalidDataThrowsException()
    {
        // Arrange

        var invalidOwnerId = "testOwnerId12";
        int expectedCount = 0;

        // Act

        // Assert
        await Assert.ThrowsExceptionAsync<TaskManagerException>(async () =>
        {
            int taskCount = await taskService.GetCountOfAll(invalidOwnerId);
        });
    }

    [TestMethod]
    public async Task UpdateById_ValidTask_UpdateSuccessful()
    {
        // Arrange
        var ownerId = "testOwnerId1";
        var existingTaskId = Guid.Parse("958bda36-754a-4358-b991-225d8de25e92");

        var updatedTask = new TaskForUpdateRequest
        {
            Id = existingTaskId,
            Name = "Updated Task",
            Description = "Updated Description",
            ImportanceLevel = "High",
            DueDate = DateTime.Now.AddDays(7)
        };

        // Act
        await taskService.UpdateById(updatedTask, ownerId);

        // Assert
        var updatedTaskFromDb = await taskService.GetById(existingTaskId, ownerId);
        Assert.IsNotNull(updatedTaskFromDb, "Task should be updated");
        Assert.AreEqual("Updated Task", updatedTaskFromDb.Name, "Task name should be updated");
    }

    [TestMethod]
    public async Task UpdateById_InvalidTask_ThrowsException()
    {
        // Arrange
        var ownerId = "testOwnerId1";
        var nonExistingTaskId = Guid.Parse("BECF18A9-6BA8-431D-AF65-299537010172");

        var updatedTask = new TaskForUpdateRequest
        {
            Id = nonExistingTaskId,
            Name = "Updated Task",
            Description = "Updated Description",
            ImportanceLevel = "High",
            DueDate = DateTime.Now.AddDays(7)
        };

        // Act and Assert
        await Assert.ThrowsExceptionAsync<TaskManagerException>(async () =>
        {
            await taskService.UpdateById(updatedTask, ownerId);
        });
    }

    [TestMethod]
    public async Task UpdateById_TaskWithPastDueDate_ThrowsException()
    {
        // Arrange
        var ownerId = "testOwnerId1";
        var existingTaskId = Guid.Parse("958bda36-754a-4358-b991-225d8de25e92");

        var updatedTask = new TaskForUpdateRequest
        {
            Id = existingTaskId,
            Name = "Updated Task",
            Description = "Updated Description",
            ImportanceLevel = "High",
            DueDate = DateTime.Now.AddDays(-1)
        };

        // Act and Assert
        await Assert.ThrowsExceptionAsync<TaskManagerException>(async () =>
        {
            await taskService.UpdateById(updatedTask, ownerId);
        });
    }

    [TestMethod]
    public async Task GetTasksBySearch_ValidData_ReturnsTasks()
    {
        // Arrange
        var ownerId = "testOwnerId2";
        var searchCriteria = "testing";

        // Act
        var result = await taskService.GetTasksBySearch(ownerId, searchCriteria);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IEnumerable<TaskDTO>));
        Assert.AreEqual(1, result.Count()); // Assuming there are 1 tasks in the mock data
    }

    [TestMethod]
    public async Task GetTasksBySearch_InvalidSearchCriteria_ReturnsTasks()
    {
        // Arrange
        var ownerId = "testOwnerId2";
        var searchCriteria = "testing1234";

        // Act and Assert
        await Assert.ThrowsExceptionAsync<TaskManagerException>(async () =>
        {
            await taskService.GetTasksBySearch(ownerId, searchCriteria);
        });
    }

    [TestMethod]
    public async Task GetTasksBySearch_InvalidOwnerId_ThrowsException()
    {
        // Arrange
        var invalidOwnerId = "invalidOwnerId";
        var searchCriteria = "someSearchCriteria";

        // Act and Assert
        await Assert.ThrowsExceptionAsync<TaskManagerException>(async () =>
        {
            await taskService.GetTasksBySearch(invalidOwnerId, searchCriteria);
        });
    }

}

