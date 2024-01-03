using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Data.Enums;
using TaskManager.Data.Models;
using TaskManager.Models;
using TaskManager.Models.Requests;
using TaskManager.Models.Requests.Task;
using TaskManager.Services.Contracts;

namespace TaskManager.Services
{
    public class TaskService : ITaskService
    {

        // Dependency Injection
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task UpdateCompletition(PatchTaskRequest request)
        {
            try
            {
                // Search Task by Id and Ownerid
                var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == request.Id && x.OwnerId == request.OwnerId);
                if (task == null) throw new ArgumentException("No task found");

                // Update completition status && save db
                task.IsCompleted = request.IsCompleted;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task Create(TaskForCreationRequest task)
        {
            try
            {
                // Create db model with the request data
                var toDoTask = new ToDoTask()
                {
                    Id = Guid.NewGuid(),
                    OwnerId = task.OwnerId,
                    Name = task.Name,
                    Description = task.Description,
                    ImportanceLevel = Enum.Parse<Importance>(task.ImportanceLevel),
                    DueDate = task.DueDate,
                };

                // Add task to db && save db
                await _context.Tasks.AddAsync(toDoTask);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task DeleteById(Guid id, string ownerId)
        {
            try
            {
                // Search task by Id
                var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
                if (!(task.OwnerId == ownerId))
                {
                    throw new Exception("You are not the owner of this task");
                }

                // Delete task && save db
                _context.Remove(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }

        public async Task<IEnumerable<TaskDTO>> GetTasksPaginated(string ownerId, int pageNumber, int pageSize)
        {
            try
            {
                // Search for all tasks from a certain page with const pageSize 8, where userId owns a task, and ordering tasks by descending
                var tasks = await _context.Tasks
                    .Where(x => x.OwnerId == ownerId)
                    .OrderByDescending(t => t.AddedDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(t => new TaskDTO
                    {
                        Id = t.Id,
                        OwnerId = t.OwnerId,
                        Name = t.Name,
                        Description = t.Description,
                        ImportanceLevel = t.ImportanceLevel,
                        IsCompleted = t.IsCompleted,
                        AddedDate = t.AddedDate,
                        DueDate = t.DueDate,
                        UpdatedDate = t.UpdatedDate
                    })
                    .ToListAsync();

                return tasks;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<TaskDTO> GetById(Guid id, string ownerId)
        {
            try
            {
                // Get one task by certain id
                var taskDb = await _context.Tasks
                    .Where(x => x.OwnerId == ownerId && x.Id == id)
                    .FirstOrDefaultAsync();
                return MapTaskEntityToTaskDTO(taskDb);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<int> GetCountOfAll(OwnerIdRequest request)
        {
            try
            {
                // Get all of the tasks
                var taskDb = await _context.Tasks.Where(t => t.OwnerId == request.OwnerId).ToListAsync();
                if (taskDb == null) throw new ArgumentException("No tasks found");

                // Get the count of the tasks
                return taskDb.Count();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task UpdateById(TaskForUpdateRequest updatedTask)
        {
            try
            {
                // Get the task that needs to be updated
                var taskDb = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == updatedTask.Id && t.OwnerId == updatedTask.OwnerId);

                if (taskDb == null)
                {
                    throw new ArgumentException("Task is null");
                }
                
                // Update the tasks properties

                taskDb.Name = updatedTask.Name;
                taskDb.Description = updatedTask.Description;
                taskDb.ImportanceLevel = Enum.Parse<Importance>(updatedTask.ImportanceLevel);
                taskDb.DueDate = updatedTask.DueDate;
                taskDb.UpdatedDate = DateTime.UtcNow;

                // Save db
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        // Helper method to map the db entity
        private TaskDTO MapTaskEntityToTaskDTO(ToDoTask taskEntity)
        {
            return new TaskDTO
            {
                Id = taskEntity.Id,
                OwnerId = taskEntity.OwnerId,
                Name = taskEntity.Name,
                Description = taskEntity.Description,
                ImportanceLevel = taskEntity.ImportanceLevel,
                IsCompleted = taskEntity.IsCompleted,
                AddedDate = taskEntity.AddedDate,
                DueDate = taskEntity.DueDate,
                UpdatedDate = taskEntity.UpdatedDate
            };
        }
    }
}
