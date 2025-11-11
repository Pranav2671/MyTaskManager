using MyTaskManager.API.Repositories;    // Access the repository interface
using MyTaskManager.Shared.Models;       // Access TaskItem model
using Microsoft.AspNetCore.Mvc;       // For ControllerBase and ActionResult

namespace MyTaskManager.API.Controllers
{
    // 1. ApiController attribute makes it a REST API controller
    // 2. Route defines URL pattern: /api/tasks

    [ApiController]
    [Route("api/[controller]")]
    public class TasksController: ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        //Constructor inject the repository
        public TasksController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }



        // ====================
        // GET: /api/tasks
        // Returns all tasks
        // ====================

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return Ok(tasks); //200 OK with task list
        }


        // ====================
        // GET: /api/tasks/{id}
        // Returns a single task by ID
        // ====================
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                return NotFound();
            return Ok(task); //200 OK with task
        }



        // ====================
        // POST: /api/tasks
        // Adds a new task
        // ====================
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
        {
            var createdTask = await _taskRepository.AddAsync(task);
            //Returns 201 Created with location header
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
        }


        // ====================
        // PUT: /api/tasks/{id}
        // Updates an existing task
        // ====================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem task)
        {
            if (id != task.Id)
                return BadRequest(); //400 Bad Request if ID mismatch

            await _taskRepository.UpdateAsync(task);
            return NoContent(); //204 No Content on success
        }

        // ====================
        // DELETE: /api/tasks/{id}
        // Deletes a task by ID
        // ====================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskRepository.DeleteAsync(id);
            return NoContent(); //204 No Content on success
        }
    }
}
