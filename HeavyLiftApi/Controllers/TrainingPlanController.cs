using HeavyLiftApi.Data;
using HeavyLiftApi.DTO;
using HeavyLiftApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HeavyLiftApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TrainingPlanController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public TrainingPlanController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpGet("GetTrainingPlans")]
        public async Task<IActionResult> GetTrainingPlans()
        {
            int userId = -1;
            bool succes = int.TryParse(User.FindFirst("UserID")?.Value, out userId);
            if (succes)
            {
                List<customtrainingplan> trainingPlans = await _context.customtrainingplans
                    .Where(tp => tp.users_id == userId)
                    .ToListAsync();
                List<ExercisePlanDTO> exercisePlanDTO = new List<ExercisePlanDTO>();
                if (trainingPlans == null || trainingPlans.Count == 0)
                {
                    return NotFound(new { message = "No training plans found for this user." });
                }
                else
                {
                    foreach (var planDB in trainingPlans)
                    {
                        exercisePlanDTO.Add(new ExercisePlanDTO(planDB));
                    }
                    return Ok(exercisePlanDTO);
                }
                
            }
            return BadRequest(new { message = "Error occured while reading userID" });
        }

        [HttpGet("DeleteTrainingPlan")]
        public async Task<IActionResult> DeleteTrainingPlan(int PlanId)
        {
            int userId = -1;
            bool succes = int.TryParse(User.FindFirst("UserID")?.Value, out userId);
            if (succes)
            {
                customtrainingplan trainingPlan = await _context.customtrainingplans.FirstOrDefaultAsync(p => p.id == PlanId && p.users_id == userId);
                if (trainingPlan == null)
                {
                    return NotFound(new { message = "No training plans found for this user." });
                }
                else
                {
                    _context.customtrainingplans.Remove(trainingPlan);
                    await _context.SaveChangesAsync();
                    return Ok("Training plan got deleted");
                }

            }
            return BadRequest(new { message = "Error occured while reading userID" });
        }

    }
}
