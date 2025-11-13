using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyTaskManager.Shared.Models;


namespace MyTaskManager.UI.Api
{
    public interface ITaskApi
    {
        [Get("/api/Tasks")]
        Task<List<TaskItem>> GetAllTasksAsync();

        [Post("/api/Tasks")]
        Task<TaskItem> CreateTaskAsync([Body] TaskItem task);
    }
}
