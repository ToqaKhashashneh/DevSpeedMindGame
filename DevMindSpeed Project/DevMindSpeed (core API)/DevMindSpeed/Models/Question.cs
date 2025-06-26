using System;
using System.Collections.Generic;

namespace DevMindSpeed.Models;

public partial class Question
{
    public int Id { get; set; }

    public int SessionId { get; set; }

    public string QuestionContent { get; set; } = null!;

    public double CorrectAnswer { get; set; }

    public double? SubmittedAnswer { get; set; }

    public DateTime? TimeStarted { get; set; }

    public DateTime? TimeSubmitted { get; set; }

    public double? TimeTakenSeconds { get; set; }

    public double? Score { get; set; }

    public virtual Session Session { get; set; } = null!;
}
