using System;
using System.Collections.Generic;

namespace HeavyLiftApi.Models;

public partial class user
{
    public int id { get; set; }

    public string email { get; set; } = null!;

    public string nickname { get; set; } = null!;

    public string password { get; set; } = null!;

    public char status { get; set; }

    public DateTime createdat { get; set; }

    public byte[]? profilepicture { get; set; }

    public virtual ICollection<customexercise> customexercises { get; set; } = new List<customexercise>();

    public virtual ICollection<customtrainingplan> customtrainingplans { get; set; } = new List<customtrainingplan>();

    public virtual ICollection<friend> friendfriendNavigations { get; set; } = new List<friend>();

    public virtual ICollection<friend> friendusers { get; set; } = new List<friend>();

    public virtual ICollection<traininghistory> traininghistories { get; set; } = new List<traininghistory>();
}
