using HeavyLiftApi.Data;
using HeavyLiftApi.DTO;
using HeavyLiftApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeavyLiftApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Login")]
        public async Task<IActionResult> LogIn()
        {
            int userId = -1;
            bool succes = int.TryParse(User.FindFirst("UserID")?.Value, out userId);
            if (succes)
            {
                return await GetUserProfile(userId);
            }
            return BadRequest(new { message = "Error occured while reading userID" });
        }

        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            if (userId <= 0) {
                return BadRequest(new { message = "Invalid userId." });
            }
            user UserData = null;

            try
            {
                UserData = await _context.users.FirstOrDefaultAsync(u => u.id == userId);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }

            if( UserData == null)
            {
                return NotFound(new { message = "This user dont exist" });
            }

            UserProfileData UserProfile = new UserProfileData(UserData);

            return Ok(UserProfile);
        }
   
    }
}
