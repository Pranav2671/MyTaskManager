namespace MyTaskManager.Shared.Models
{
    public class AuthUser
    {
        public int Id { get; set; }                 // Primary Key
        public string? Username { get; set; }        // Username
        public string? PasswordHash { get; set; }    // Hashed password (not plain)
    }
}
