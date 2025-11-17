using Microsoft.AspNetCore.Mvc;
using MyTaskManager.API.Data;
using MyTaskManager.Shared.Models;
using BCrypt.Net;
using System.Linq;

namespace MyTaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (_context.Users.Any(u => u.Username == request.Username))
                return BadRequest("Username already exists.");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { Message = "User registered successfully." });
        }

        // --- ADD THIS LOGIN ACTION ---
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username))
                return BadRequest("Invalid request.");

            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);
            if (user == null)
                return Unauthorized("Invalid username or password.");

            bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isValid)
                return Unauthorized("Invalid username or password.");

            // Return only safe fields (no password hash)
            return Ok(new
            {
                id = user.Id,
                username = user.Username
            });
        }
    }
}
