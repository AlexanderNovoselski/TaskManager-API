using TaskManager.Models;
using TaskManager.Models.Requests;

namespace TaskManager.Services.Contracts
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDTO>> GetTasksPaginated(string ownerId, int pageNumber, int pageSize);

        Task<TaskDTO> GetById(Guid id, string ownerId);

        Task UpdateById(TaskForUpdateRequest updatedTask);

        Task Create(TaskForCreationRequest toDoTask);

        Task DeleteById(Guid id, string ownerId);

        Task<int> GetCountOfAll(OwnerIdRequest ownerId);

        Task UpdateCompletition(PatchTaskRequest request);
    }
}
