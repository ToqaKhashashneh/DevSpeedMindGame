using System;
using System.Collections.Generic;

namespace DevMindSpeed.Models;

public partial class Session
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Difficulty { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public double? TotalTimeSpentSeconds { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
