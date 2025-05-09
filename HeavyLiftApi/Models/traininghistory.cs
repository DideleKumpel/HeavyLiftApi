using System;
using System.Collections.Generic;

namespace HeavyLiftApi.Models;

public partial class traininghistory
{
    public int id { get; set; }

    public List<ExerciseEntry> plan { get; set; } = null!;

    public string name { get; set; } = null!;

    public DateOnly date { get; set; }

    public TimeSpan time { get; set; }

    public int users_id { get; set; }

    public virtual user users { get; set; } = null!;
}
