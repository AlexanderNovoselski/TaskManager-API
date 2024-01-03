using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.UnitTesting
{
    public class TaskServiceTests
    {
        private Mock<ApplicationDbContext> mockDbContext;
        private Mock<DbSet<TaskManager.Data.Models.ToDoTask>> mockTaskSet;

        [SetUp]
        public void Setup()
        {
            // Initialize mocks before each test
            mockDbContext = new Mock<ApplicationDbContext>();
            mockTaskSet = new Mock<DbSet<TaskManager.Data.Models.ToDoTask>>();

            // Mocking the behavior of Tasks property in the context
            mockDbContext.Setup(c => c.Tasks).Returns(mockTaskSet.Object);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}