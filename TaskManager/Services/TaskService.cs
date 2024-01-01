using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManager.Data;
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

        public Task<TaskForDeletionDTO> DeleteById(string id)
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
            catch (Exception)
            {
                throw;
            }
        }

        public Task<TaskDTO> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<TaskForUpdateDTO> UpdateById(string id, TaskForUpdateDTO updatedTask)
        {
            throw new NotImplementedException();
        }
    }
}
