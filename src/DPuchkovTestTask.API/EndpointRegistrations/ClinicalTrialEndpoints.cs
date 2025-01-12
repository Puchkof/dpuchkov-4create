using Asp.Versioning;
using Asp.Versioning.Builder;
using MediatR;
using DPuchkovTestTask.Application.Files.Commands.UploadFile;
using DPuchkovTestTask.Application.Trials.Queries.GetTrialById;
using DPuchkovTestTask.Application.Trials.Queries.GetTrialsList;
using DPuchkovTestTask.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace DPuchkovTestTask.API.EndpointRegistrations;

public static class ClinicalTrialEndpoints
{
    public static WebApplication AddClinicalTrialEndpoints(this WebApplication app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();
        
        app.MapPost(Routes.UploadFile, async (
            IFormFile? file,
            [FromServices] ISender sender) =>
        {
            if (file == null || file.Length == 0)
                return Results.BadRequest("No file was uploaded.");

            var fileStream = file.OpenReadStream();
            var command = new UploadFileCommand(
                fileStream,
                file.FileName,
                file.Length);

            var result = await sender.Send(command);

            return result.IsSuccess 
                ? Results.Ok(new { message = result.Message })
                : Results.BadRequest(result.Message);
        })
        .DisableAntiforgery() // For test task only
        .WithName("UploadFile")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Upload JSON file",
            Description = "Uploads a JSON file with clinical trials."
        })
        .WithApiVersionSet(apiVersionSet);

        app.MapGet($"{Routes.Trials}/{{trialId}}", async (
            string trialId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTrialByIdQuery(trialId);
            var result = await sender.Send(query, cancellationToken);
            return Results.Ok(result);
        })
        .WithName("GetTrialById")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Get trial by ID",
            Description = "Retrieves a specific clinical trial by its ID"
        })
        .WithApiVersionSet(apiVersionSet);

        app.MapGet(Routes.Trials, async (
            [AsParameters] GetTrialsListQuery query,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(query, cancellationToken);
            return Results.Ok(result);
        })
        .WithName("GetTrials")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Get trials list",
            Description = "Retrieves a list of clinical trials, optionally filtered by status"
        })
        .WithApiVersionSet(apiVersionSet);

        return app;
    }
}