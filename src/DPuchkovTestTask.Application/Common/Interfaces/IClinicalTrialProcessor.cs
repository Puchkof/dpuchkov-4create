using DPuchkovTestTask.Domain.Entities;
using DPuchkovTestTask.Application.Files.Models;

namespace DPuchkovTestTask.Application.Common.Interfaces;

public interface IClinicalTrialProcessor
{
    ClinicalTrial ProcessTrial(ClinicalTrialDto dto);
} 