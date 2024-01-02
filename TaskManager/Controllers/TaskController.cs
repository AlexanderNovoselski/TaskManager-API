
using Microsoft.AspNetCore.Mvc;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.Requests;
using TaskManager.Services.Contracts;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ApplicationDbContext _context;

        public TaskController(ITaskService taskService, ApplicationDbContext context)
        {
            _taskService = taskService;
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTasks([FromBody] OwnerIdRequest request)
        {
            try
            {
                var tasks = await _taskService.GetAll(request.OwnerId);
                if (!tasks.Any())
                {
                    throw new Exception();
                }
                return Ok(tasks);
            }
            catch (Exception)
            {
                return NotFound();
            }

        }

        [HttpGet("GetTask")]
        public async Task<ActionResult<TaskDTO>> GetToDoTask([FromBody] TaskIdOwnerIdRequest request)
        {
            try
            {
                var task = await _taskService.GetById(request.Id, request.OwnerId);

                if (task == null)
                {
                    return NotFound();
                }

                return Ok(task);
            }
            catch (Exception)
            {
                return NotFound();
            }

        }

        [HttpPut("Update")]
        public async Task<IActionResult> PutToDoTask([FromBody] TaskForUpdateRequest request)
        {
            try
            {
                await _taskService.UpdateById(request);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult> PostToDoTask([FromBody] TaskForCreationRequest request)
        {
            try
            {
                await _taskService.Create(request);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Failed to create the task.");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteToDoTask([FromBody] TaskIdOwnerIdRequest request)
        {
            try
            {
                await _taskService.DeleteById(request.Id, request.OwnerId);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }

        }

        [HttpPatch("Update")]

        public async Task<IActionResult> UpdateCompletition([FromBody] PatchTaskRequest request)
        {
            try
            {
                await _taskService.UpdateCompletition(request);
                var result = new 
                { 
                    id = request.Id,
                    ownerId = request.OwnerId,
                    TaskCompletition = request.IsCompleted 
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("GetCount")]

        public async Task<IActionResult> GetTotalCount([FromBody] OwnerIdRequest request)
        {
            try
            {
                var count = await _taskService.GetCountOfAll(request);
                var result = new { Count = count };

                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound("No tasks founds");
            }

        }

    }
}
