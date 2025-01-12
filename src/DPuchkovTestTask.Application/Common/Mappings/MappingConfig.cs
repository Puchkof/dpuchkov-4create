using DPuchkovTestTask.Application.Files.Models;
using DPuchkovTestTask.Domain.Entities;
using Mapster;

namespace DPuchkovTestTask.Application.Common.Mappings;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ClinicalTrial, ClinicalTrialDto>();
    }
} 