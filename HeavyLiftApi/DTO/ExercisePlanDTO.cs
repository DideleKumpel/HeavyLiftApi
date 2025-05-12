using HeavyLiftApi.Models;

namespace HeavyLiftApi.DTO
{
        public class ExercisePlanDTO
        {
            public int id { get; set; }
            public List<ExerciseEntry> plan { get; set; } = null!;

            public string name { get; set; } = null!;

            public ExercisePlanDTO(customtrainingplan plan)
            {
                id = plan.id;
                name = plan.name;
                this.plan = plan.plan;
            }
        }
}
