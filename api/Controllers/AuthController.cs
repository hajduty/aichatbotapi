using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login(User login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = _context.Users.Find(login.Email);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Password != login.Password)
            {
                return Unauthorized();
            }

            // Skicka med jwt token

            return Ok();
        }

        [HttpPost("register")]
        public IActionResult Register(User register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = _context.Users.Find(register.Email);

            if (user != null)
            {
                return Conflict();
            }

            _context.Users.Add(register);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Login), new { email = register.Email }, register);
        }
    }
}
