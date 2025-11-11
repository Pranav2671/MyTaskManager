namespace MyTaskManager.Shared.Models
{
    public class AuthResponse
    {
        public required string Token { get; set; }         // JWT token
        public required string Username { get; set; }
        public long ExpiresAtUnix { get; set; }   // expiry time as Unix timestamp
    }
}
