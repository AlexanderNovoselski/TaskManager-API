using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Threading.Tasks;
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
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task UpdateCompletition(PatchTaskRequest request)
        {
            try
            {
                var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == request.Id && x.OwnerId == request.OwnerId);
                if (task == null) throw new ArgumentException("No task found");

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
                var toDoTask = new ToDoTask()
                {
                    Id = Guid.NewGuid(),
                    OwnerId = task.OwnerId,
                    Name = task.Name,
                    Description = task.Description,
                    ImportanceLevel = Enum.Parse<Importance>(task.ImportanceLevel),
                    DueDate = task.DueDate,
                };
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
                var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
                if (!(task.OwnerId == ownerId))
                {
                    throw new Exception("You are not the owner of this task");
                }

                _context.Remove(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }

        public async Task<IEnumerable<TaskDTO>> GetAll(string ownerId)
        {
            try
            {
                return await _context.Tasks.Where(x => x.OwnerId == ownerId).Select(t => new TaskDTO
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
                }).ToListAsync();
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
                var taskDb = await _context.Tasks.Where(t => t.OwnerId == request.OwnerId).ToListAsync();
                if (taskDb == null) throw new ArgumentException("No tasks found");
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
                var taskDb = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == updatedTask.Id && t.OwnerId == updatedTask.OwnerId);

                if (taskDb == null)
                {
                    throw new ArgumentException("Task is null");
                }
                taskDb.Name = updatedTask.Name;
                taskDb.Description = updatedTask.Description;
                taskDb.ImportanceLevel = Enum.Parse<Importance>(updatedTask.ImportanceLevel);
                taskDb.DueDate = updatedTask.DueDate;
                taskDb.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

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
