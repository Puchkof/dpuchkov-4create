using DPuchkovTestTask.Application.Files.Models;
using DPuchkovTestTask.Domain.Enums;
using MediatR;

namespace DPuchkovTestTask.Application.Trials.Queries.GetTrialsList;

public record GetTrialsListQuery(TrialStatus? Status) : IRequest<IEnumerable<ClinicalTrialDto>>;