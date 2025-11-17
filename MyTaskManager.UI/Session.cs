using MyTaskManager.Shared.Models;

namespace MyTaskManager.UI
{
    public static class Session
    {
        // This will store the logged-in user's info
        public static User? CurrentUser { get; set; }
    }
}
