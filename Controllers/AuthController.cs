using Microsoft.AspNetCore.Mvc;
using orion.Models;
using orion.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace orion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly OrionDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthController(OrionDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            try
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var newUser = new User
                {
                    Username = request.Username,
                    PasswordHash = passwordHash
                };

                // Save the new user to the database
                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                // Log or display the exception message and inner exception
                Console.WriteLine($"An error occurred while saving the entity changes: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException}");

                // Return an appropriate response to the client
                return StatusCode(500, "An error occurred while saving the entity changes. Please try again later.");
            }
        }

        private int GenerateUserId()
        {
            // Your logic to generate a unique Id for the user
            // You can use various methods, such as using a database-generated Id or a custom logic

            // For simplicity, you can use a random number generator for now
            Random random = new Random();
            return random.Next(1000, 9999);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDto request)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == request.Username);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password");
            }

            string token = CreateToken(user);

            return Ok(token);
        }


        //log out
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            // Perform any server-side cleanup if necessary

            // Return a response to the client indicating successful logout
            return Ok("Logout successful");
        }


        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}