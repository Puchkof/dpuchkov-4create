using DPuchkovTestTask.Domain.Enums;

namespace DPuchkovTestTask.Domain.Entities;

public class ClinicalTrial
{
    public int Id { get; set; }
    public string TrialId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? Participants { get; set; }
    public TrialStatus Status { get; set; }
    public int DurationInDays { get; set; }
} 