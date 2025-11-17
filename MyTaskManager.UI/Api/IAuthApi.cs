using MyTaskManager.Shared.Models;
using Refit;
using System.Threading.Tasks;

namespace MyTaskManager.UI.Api
{
    public interface IAuthApi
    {
        [Post("/api/Auth/login")]
        Task<User> LoginAsync([Body] LoginRequest request);

        [Post("/api/Auth/register")]
        Task RegisterAsync([Body] RegisterRequest request);
    }
}
