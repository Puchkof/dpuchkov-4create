using System.Text.Json;
using DPuchkovTestTask.Application.Common.Interfaces;
using DPuchkovTestTask.Application.Files.Models;
using DPuchkovTestTask.Infrastructure.Interfaces;
using DPuchkovTestTask.Infrastructure.Persistence;
using MediatR;

namespace DPuchkovTestTask.Application.Files.Commands.UploadFile;

public class UploadFileCommandHandler(
    IJsonSchemaValidator schemaValidator,
    IClinicalTrialProcessor trialProcessor,
    ApplicationDbContext context)
    : IRequestHandler<UploadFileCommand, UploadFileResult>
{
    public async Task<UploadFileResult> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            using var streamReader = new StreamReader(request.FileStream);
            var jsonContent = await streamReader.ReadToEndAsync(cancellationToken);

            var (isValid, errors) = await schemaValidator.ValidateAsync(jsonContent, cancellationToken);
            if (!isValid)
            {
                return new UploadFileResult(false, 
                    $"JSON validation failed: {string.Join(", ", errors)}");
            }

            var trials = JsonSerializer.Deserialize<List<ClinicalTrialDto>>(jsonContent,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            if (trials == null)
            {
                return new UploadFileResult(false, "Failed to deserialize JSON content");
            }

            var processedTrials = trials.Select(trialProcessor.ProcessTrial);
            
            await context.ClinicalTrials.AddRangeAsync(processedTrials, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return new UploadFileResult(true, $"Successfully processed and stored {trials.Count} trials");
        }
        catch (Exception ex)
        {
            return new UploadFileResult(false, $"Error processing file: {ex.Message}");
        }
    }
}