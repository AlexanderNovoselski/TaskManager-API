
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManager.Data;
using TaskManager.Data.Models;
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

        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutToDoTask(Guid id, ToDoTask toDoTask)
        {
            if (id != toDoTask.Id)
            {
                return BadRequest();
            }

            _context.Entry(toDoTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return Ok();
        }

        [HttpPost("create")]
        public async Task<ActionResult<ToDoTask>> PostToDoTask(ToDoTask toDoTask)
        {
            if (_context.Tasks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tasks'  is null.");
            }
            _context.Tasks.Add(toDoTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDoTask", new { id = toDoTask.Id }, toDoTask);
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


    }

}
