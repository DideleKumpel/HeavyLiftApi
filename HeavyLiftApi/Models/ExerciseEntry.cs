namespace HeavyLiftApi.Models
{
    public class ExerciseEntry
    {
        public string name { get; set; } = null!;

        public string description { get; set; } = null!;

        public string musclegroups { get; set; } = null!;

        public string type { get; set; } = null!;

        public byte[] image { get; set; } = null!;

        public List<RepEntry> reps { get; set; } = null!;
    }
}
