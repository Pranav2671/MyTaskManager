using MyTaskManager.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTaskManager.API.Repositories
{
    // Interface defining basic operations for TaskItem
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();   // Get all tasks
        Task<TaskItem?> GetByIdAsync(int id);         // Get task by ID
        Task<TaskItem> AddAsync(TaskItem task);      // Add new task
        Task UpdateAsync(TaskItem task);             // Update task
        Task DeleteAsync(int id);                    // Delete task
    }
}
