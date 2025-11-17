namespace MyTaskManager.Shared.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        // BCrypt hash string (already includes salt inside)
        public string PasswordHash { get; set; }
    }
}
