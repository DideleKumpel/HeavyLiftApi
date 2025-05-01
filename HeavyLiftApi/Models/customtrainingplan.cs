using System;
using System.Collections.Generic;

namespace HeavyLiftApi.Models;

public partial class customtrainingplan
{
    public int id { get; set; }

    public List<ExerciseEntry> plan { get; set; } = null!;

    public int users_id { get; set; }

    public virtual user users { get; set; } = null!;
}
