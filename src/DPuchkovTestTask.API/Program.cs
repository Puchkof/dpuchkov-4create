using System.Text.Json;
using Asp.Versioning;
using DPuchkovTestTask.API.EndpointRegistrations;
using DPuchkovTestTask.Application.Common.Behaviors;
using DPuchkovTestTask.Application.Files.Commands.UploadFile;
using FluentValidation;
using MediatR;
using DPuchkovTestTask.API.ExceptionHandlers;
using DPuchkovTestTask.Application.Common.Interfaces;
using DPuchkovTestTask.Application.Common.Mappings;
using DPuchkovTestTask.Infrastructure.Interfaces;
using DPuchkovTestTask.Infrastructure.Services;
using DPuchkovTestTask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(UploadFileCommand).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
});

// Register validators
builder.Services.AddValidatorsFromAssembly(typeof(UploadFileCommand).Assembly);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<IJsonSchemaValidator, JsonSchemaValidator>();

builder.Services.AddScoped<IClinicalTrialProcessor, ClinicalTrialProcessor>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Mapster
var config = TypeAdapterConfig.GlobalSettings;
config.Scan(typeof(MappingConfig).Assembly);
builder.Services.AddSingleton(config);

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1.0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.AddClinicalTrialEndpoints();

app.UseExceptionHandler();

await ApplicationDbInitializer.InitializeDatabaseAsync(app);

app.Run();

public partial class Program { }