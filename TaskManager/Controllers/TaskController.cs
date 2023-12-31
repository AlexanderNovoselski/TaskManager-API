﻿
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Data.Models;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetTasks()
        {
            var tasks = await _context.Tasks.ToListAsync();

            if (tasks == null || tasks.Count == 0)
            {
                return NotFound();
            }

            return Ok(tasks);
        }

        [HttpGet("GetTask/{id}")]
        public async Task<ActionResult<ToDoTask>> GetToDoTask(Guid id)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            var toDoTask = await _context.Tasks.FindAsync(id);

            if (toDoTask == null)
            {
                return NotFound();
            }

            return Ok(toDoTask);
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
                if (!ToDoTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
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

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteToDoTask(Guid id)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            var toDoTask = await _context.Tasks.FindAsync(id);
            if (toDoTask == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(toDoTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToDoTaskExists(Guid id)
        {
            return (_context.Tasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
