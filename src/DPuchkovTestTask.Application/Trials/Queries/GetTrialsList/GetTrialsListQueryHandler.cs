using DPuchkovTestTask.Application.Files.Models;
using DPuchkovTestTask.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPuchkovTestTask.Application.Trials.Queries.GetTrialsList;

public class GetTrialsListQueryHandler : IRequestHandler<GetTrialsListQuery, IEnumerable<ClinicalTrialDto>>
{
    private readonly ApplicationDbContext _context;

    public GetTrialsListQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClinicalTrialDto>> Handle(GetTrialsListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ClinicalTrials.AsNoTracking();

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status.Value);
        }

        var trials = await query.ToListAsync(cancellationToken);
        
        return trials.Adapt<IEnumerable<ClinicalTrialDto>>();
    }
} 