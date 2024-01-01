using TaskManager.Models;

namespace TaskManager.Services.Contracts
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDTO>> GetAll();

        Task<TaskDTO> GetById(string id);

        Task<TaskForUpdateDTO> UpdateById(string id, TaskForUpdateDTO updatedTask);

        Task<TaskForCreationDTO> Create(TaskForCreationDTO toDoTask);

        Task<TaskForDeletionDTO> DeleteById(string id);
    }
}
