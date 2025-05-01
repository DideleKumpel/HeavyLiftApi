using HeavyLiftApi.Data;
using HeavyLiftApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace HeavyLiftApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "LogIn")]
        public async Task<user> LogIn(string email, string password)
        {
            user LogInUser = new user() { 
                id = 1,
                nickname = "TestUser",
                email = email,
                password = password,
                status = 'A',
                createdat = DateTime.Now,
                profilepicture = null,
            };
            return LogInUser;
        }
   
    }
}
