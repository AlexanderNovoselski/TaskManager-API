
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Models;
using TaskManager.Models.Requests;
using TaskManager.Models.Requests.Task;
using TaskManager.Services.Contracts;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : BaseApiController
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [Authorize]
        [HttpGet("Search")]
        public async Task<IActionResult> GetTasksBySearch([FromQuery] string searchCriteria)
        {
            try
            {
                // Call the service method to get tasks by search criteria
                var tasks = await _taskService.GetTasksBySearch(OwnerId, searchCriteria);


                if (!tasks.Any())
                {
                    return NotFound("Tasks not found");
                }

                // Return the result as JSON
                return Ok(tasks);
            }
            catch (TaskManagerException ex)
            {
                // Handle specific exceptions if needed
                return BadRequest(new { Message = ex.Message });
            }

        }

        [Authorize]
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetAllTasks([FromQuery] int pageNumber = 1)
        {
            const int pageSize = 5;
            try
            {
                var tasks = await _taskService.GetTasksPaginated(OwnerId, pageNumber, pageSize);

                if (!tasks.Any())
                {
                    return NotFound("Tasks not found");
                }

                return Ok(tasks);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize]
        [HttpGet("GetAllIncompleted")]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetAllNonCompletedTasks([FromQuery] int pageNumber = 1)
        {
            const int pageSize = 8;
            try
            {
                var tasks = await _taskService.GetNonCompletedTasksPaginated(OwnerId, pageNumber, pageSize);

                if (!tasks.Any())
                {
                    return NotFound("Tasks not found");
                }

                return Ok(tasks);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize]
        [HttpGet("GetTask")]
        public async Task<ActionResult<TaskDTO>> GetToDoTask([FromBody] TaskIdOwnerIdRequest request)
        {
            try
            {
                var task = await _taskService.GetById(request.Id, OwnerId);

                if (task == null)
                {
                    throw new ArgumentException();
                }

                return Ok(task);
            }
            catch (Exception)
            {
                return NotFound("Task not found");
            }

        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> PutToDoTask([FromBody] TaskForUpdateRequest request)
        {
            try
            {
                await _taskService.UpdateById(request, OwnerId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<ActionResult> PostToDoTask([FromBody] TaskForCreationRequest request)
        {
            try
            {
                await _taskService.Create(request, OwnerId);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Failed to create the task.");
            }
        }

        [Authorize]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteToDoTask([FromBody] TaskIdOwnerIdRequest request)
        {
            try
            {
                await _taskService.DeleteById(request.Id, OwnerId);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound("Task not found");
            }

        }

        [Authorize]
        [HttpPatch("UpdateCompletion")]

        public async Task<IActionResult> UpdateCompletition([FromBody] PatchTaskRequest request)
        {
            try
            {
                await _taskService.UpdateCompletition(request, OwnerId);
                var result = new 
                { 
                    id = request.Id,
                    ownerId = OwnerId,
                    TaskCompletition = request.IsCompleted 
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound("Task not found");
            }
        }

        [Authorize]
        [HttpGet("GetCount")]

        public async Task<IActionResult> GetTotalCount()
        {
            try
            {
                var count = await _taskService.GetCountOfAll(OwnerId);
                var result = new { Count = count };

                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound("Tasks not found");
            }

        }

    }
}
