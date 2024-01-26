using TaskManager.Models;
using TaskManager.Models.Requests;
using TaskManager.Models.Requests.Task;

namespace TaskManager.Services.Contracts
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDTO>> GetTasksBySearch(string ownerId, string searchCriteria);
        Task<IEnumerable<TaskDTO>> GetTasksPaginated(string ownerId, int pageNumber, int pageSize);
        Task<IEnumerable<TaskDTO>> GetNonCompletedTasks(string ownerId, DateTime clickedDate);

        Task<TaskDTO> GetById(Guid id, string ownerId);

        Task UpdateById(TaskForUpdateRequest updatedTask, string ownerId);

        Task Create(TaskForCreationRequest toDoTask, string ownerId);

        Task DeleteById(Guid id, string ownerId);

        Task<int> GetCountOfAll(string ownerId);
        Task<int> GetCountByDate(string ownerId, DateTime clickedDate);

        Task UpdateCompletition(PatchTaskRequest request, string ownerId);
    }
}
