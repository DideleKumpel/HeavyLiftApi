using System;
using System.Collections.Generic;

namespace HeavyLiftApi.Models;

public partial class friend
{
    public int id { get; set; }

    public int userid { get; set; }

    public int friendid { get; set; }

    public string status { get; set; } = null!;

    public virtual user friendNavigation { get; set; } = null!;

    public virtual user user { get; set; } = null!;
}
