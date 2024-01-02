﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Models.Requests;
using TaskManager.Models.Requests.Task;
using TaskManager.Services.Contracts;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [Authorize]
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTasks([FromBody] OwnerIdRequest request, [FromQuery] int pageNumber = 1)
        {
            const int pageSize = 8;
            try
            {
                var tasks = await _taskService.GetTasksPaginated(request.OwnerId, pageNumber, pageSize);

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
                var task = await _taskService.GetById(request.Id, request.OwnerId);

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
                await _taskService.UpdateById(request);

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
                await _taskService.Create(request);

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
                await _taskService.DeleteById(request.Id, request.OwnerId);
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
                return NotFound("Task not found");
            }
        }

        [Authorize]
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
                return NotFound("Tasks not found");
            }

        }

    }
}
