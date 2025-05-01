using System;
using System.Collections.Generic;

namespace HeavyLiftApi.Models;

public partial class customexercise
{
    public int id { get; set; }

    public string name { get; set; } = null!;

    public string? description { get; set; }

    public string musclegroups { get; set; } = null!;

    public string type { get; set; } = null!;

    public byte[]? image { get; set; }

    public int users_id { get; set; }

    public virtual user users { get; set; } = null!;
}
