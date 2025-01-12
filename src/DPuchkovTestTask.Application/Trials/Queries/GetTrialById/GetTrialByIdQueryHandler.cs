using DPuchkovTestTask.Application.Common.Exceptions;
using DPuchkovTestTask.Application.Files.Models;
using DPuchkovTestTask.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPuchkovTestTask.Application.Trials.Queries.GetTrialById;

public class GetTrialByIdQueryHandler : IRequestHandler<GetTrialByIdQuery, ClinicalTrialDto>
{
    private readonly ApplicationDbContext _context;

    public GetTrialByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ClinicalTrialDto> Handle(GetTrialByIdQuery request, CancellationToken cancellationToken)
    {
        var trial = await _context.ClinicalTrials
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TrialId == request.TrialId, cancellationToken);

        if (trial == null)
        {
            throw new NotFoundException($"Trial with ID {request.TrialId} was not found.");
        }

        return trial.Adapt<ClinicalTrialDto>();
    }
} 