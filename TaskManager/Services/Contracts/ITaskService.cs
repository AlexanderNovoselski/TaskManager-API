using TaskManager.Models;

namespace TaskManager.Services.Contracts
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDTO>> GetAll(string ownerId);

        Task<TaskDTO> GetById(Guid id, string ownerId);

        Task<TaskForUpdateDTO> UpdateById(Guid id, TaskForUpdateDTO updatedTask);

        Task<TaskForCreationDTO> Create(TaskForCreationDTO toDoTask);

        Task DeleteById(Guid id, string ownerId);
    }
}
