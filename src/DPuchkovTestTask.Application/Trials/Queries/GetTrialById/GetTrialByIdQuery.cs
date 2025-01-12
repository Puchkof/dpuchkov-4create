using DPuchkovTestTask.Application.Files.Models;
using MediatR;

namespace DPuchkovTestTask.Application.Trials.Queries.GetTrialById;

public record GetTrialByIdQuery(string TrialId) : IRequest<ClinicalTrialDto>; 