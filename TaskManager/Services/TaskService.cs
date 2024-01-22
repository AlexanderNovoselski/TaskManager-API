using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TaskManager.Data;
using TaskManager.Data.Enums;
using TaskManager.Data.Models;
using TaskManager.Models;
using TaskManager.Models.Requests;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new TaskManagerException("Error retrieving paginated tasks", ex);
            }
        }

        public async Task<IEnumerable<TaskDTO>> GetNonCompletedTasks(string ownerId, DateTime clickedDate)
        {
            try
            {
                // Search for all tasks from a certain page with const pageSize 8, where userId owns a task, and ordering tasks by descending
                int day = clickedDate.Day;
                int month = clickedDate.Month;
                int year = clickedDate.Year;

                // Create a new DateTime with the same day, month, and year, but with time set to midnight
                DateTime startDate = new DateTime(year, month, day, 0, 0, 0);

                // Create a new DateTime representing the end of the day
                DateTime endDate = startDate.AddDays(1).AddTicks(-1);

                var tasks = await _context.Tasks
                    .Where(x => x.OwnerId == ownerId && x.IsCompleted == false && x.DueDate >= startDate && x.DueDate <= endDate)
                    .OrderByDescending(t => t.AddedDate)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new TaskManagerException("Error retrieving tasks", ex);
            }
        }

        public async Task<IEnumerable<TaskDTO>> GetTasksBySearch(string ownerId, string searchCriteria)
        {
            try
            {
                // Search for tasks based on the provided criteria, where userId owns a task
                var tasks = await _context.Tasks
                    .Where(x => x.OwnerId == ownerId && (x.Name.Contains(searchCriteria) || x.Description.Contains(searchCriteria)))
                    .OrderByDescending(t => t.AddedDate)
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

                if (tasks.Count == 0)
                {
                    // No tasks found based on the search criteria
                    throw new TaskManagerException("No tasks found based on the provided criteria.");
                }

                return tasks;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new TaskManagerException("Error retrieving tasks by search criteria", ex);
            }
        }
        public async Task UpdateCompletition(PatchTaskRequest request, string ownerId)
        {
            try
            {
                var task = await _context.Tasks
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.OwnerId == ownerId);

                if (task == null)
                {
                    throw new TaskManagerException("Task not found");
                }

                task.IsCompleted = request.IsCompleted;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new TaskManagerException("Error updating completion status", ex);
            }
        }

        public async Task Create(TaskForCreationRequest task, string ownerId)
        {
            try
            {
                // Validate the task manually
                var validationContext = new ValidationContext(task, serviceProvider: null, items: null);
                Validator.ValidateObject(task, validationContext, validateAllProperties: true);

                // Create db model with the request data
                var toDoTask = new ToDoTask()
                {
                    Id = Guid.NewGuid(),
                    OwnerId = ownerId,
                    Name = task.Name,
                    Description = task.Description,
                    ImportanceLevel = Enum.Parse<Importance>(task.ImportanceLevel),
                    DueDate = task.DueDate,
                };

                // Add task to db && save db
                await _context.Tasks.AddAsync(toDoTask);
                await _context.SaveChangesAsync();
            }
            catch (ValidationException ex)
            {
                Console.WriteLine(ex.Message);
                throw new TaskManagerException("Validation error while creating the task", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new TaskManagerException("Error while creating the task", ex);
            }
        }

        public async Task DeleteById(Guid id, string ownerId)
        {
            try
            {
                var task = _context.Tasks.FirstOrDefault(t => t.Id == id);

                if (task == null)
                {
                    throw new TaskManagerException("Task not found");
                }

                if (task.OwnerId != ownerId)
                {
                    throw new TaskManagerException("You are not the owner of this task");
                }

                _context.Remove(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new TaskManagerException("Error deleting task", ex);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new TaskManagerException("Error retrieving the specified task", ex);
            }
        }

        public async Task<int> GetCountOfAll(string ownerId)
        {
            try
            {
                // Get all of the tasks
                var taskDb = await _context.Tasks.Where(t => t.OwnerId == ownerId).ToListAsync();
                if (taskDb == null || taskDb.Count == 0)
                {
                    throw new TaskManagerException("No tasks found");
                }

                // Get the count of the tasks
                return taskDb.Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new TaskManagerException("Error retrieving the count of the tasks", ex);
            }
        }

        public async Task UpdateById(TaskForUpdateRequest updatedTask, string ownerId)
        {

            try
            {
                // Validate the updatedTask manually
                var validationContext = new ValidationContext(updatedTask, serviceProvider: null, items: null);
                Validator.ValidateObject(updatedTask, validationContext, validateAllProperties: true);

                // Get the task that needs to be updated
                var taskDb = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == updatedTask.Id && t.OwnerId == ownerId);

                if (taskDb == null)
                {
                    throw new TaskManagerException("Task not found for update");
                }

                // Update the tasks properties
                taskDb.Name = updatedTask.Name;
                taskDb.Description = updatedTask.Description;
                taskDb.ImportanceLevel = Enum.Parse<Importance>(updatedTask.ImportanceLevel);

                // Validate DueDate after the update
                Validator.ValidateProperty(updatedTask.DueDate, new ValidationContext(updatedTask) { MemberName = nameof(updatedTask.DueDate) });

                taskDb.DueDate = updatedTask.DueDate;
                taskDb.UpdatedDate = DateTime.UtcNow;

                // Save db
                await _context.SaveChangesAsync();
            }
            catch (ValidationException ex)
            {
                Console.WriteLine(ex.Message);
                throw new TaskManagerException("Error updating task", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new TaskManagerException("Error updating task", ex);
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
