using System.Text.Json.Serialization;
using DPuchkovTestTask.Domain.Enums;
using Newtonsoft.Json.Serialization;

namespace DPuchkovTestTask.Application.Files.Models;

public class ClinicalTrialDto
{
    public string TrialId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? Participants { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TrialStatus Status { get; set; }
} 