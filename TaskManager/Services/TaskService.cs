using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManager.Data;
using TaskManager.Data.Models;
using TaskManager.Models;
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
        public Task<TaskForCreationDTO> Create(TaskForCreationDTO toDoTask)
        {
            throw new NotImplementedException();
        }

        public Task<TaskForDeletionDTO> DeleteById(Guid id)
        {
            throw new NotImplementedException();
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

        public Task<TaskForUpdateDTO> UpdateById(Guid id, TaskForUpdateDTO updatedTask)
        {
            throw new NotImplementedException();
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
