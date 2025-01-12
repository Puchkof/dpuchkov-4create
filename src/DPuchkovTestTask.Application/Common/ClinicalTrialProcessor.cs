using DPuchkovTestTask.Application.Common.Interfaces;
using DPuchkovTestTask.Application.Files.Models;
using DPuchkovTestTask.Domain.Entities;
using DPuchkovTestTask.Domain.Enums;

namespace DPuchkovTestTask.Infrastructure.Services;

public class ClinicalTrialProcessor : IClinicalTrialProcessor
{
    public ClinicalTrial ProcessTrial(ClinicalTrialDto dto)
    {
        var endDate = dto.EndDate;
        
        if (endDate == null && dto.Status == TrialStatus.Ongoing)
        {
            endDate = dto.StartDate.AddMonths(1);
        }
        
        var durationInDays = endDate.HasValue 
            ? (int)(endDate.Value - dto.StartDate).TotalDays 
            : (int)(DateTime.UtcNow - dto.StartDate).TotalDays;

        return new ClinicalTrial
        {
            TrialId = dto.TrialId,
            Title = dto.Title,
            StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
            EndDate = endDate.HasValue ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc) : null,
            Participants = dto.Participants,
            Status = dto.Status,
            DurationInDays = durationInDays
        };
    }
} 