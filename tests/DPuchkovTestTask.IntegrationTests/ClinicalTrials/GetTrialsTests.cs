using System.Net;
using System.Net.Http.Json;
using DPuchkovTestTask.API.EndpointRegistrations;
using DPuchkovTestTask.Application.Files.Models;
using DPuchkovTestTask.Domain.Entities;
using DPuchkovTestTask.Domain.Enums;
using DPuchkovTestTask.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace DPuchkovTestTask.IntegrationTests.ClinicalTrials;

public class GetTrialsTests : IntegrationTestBase
{
    [Fact]
    public async Task GetTrialById_ExistingTrial_ShouldReturnTrial()
    {
        // Arrange
        var trial = await CreateTestTrial();

        // Act
        var response = await Client.GetAsync(Routes.GetVersionedRoute($"{Routes.Trials}/{trial.TrialId}", 1));
        var result = await response.Content.ReadFromJsonAsync<ClinicalTrialDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.TrialId.Should().Be(trial.TrialId);
    }
    
    [Fact]
    public async Task GetTrialById_NonExistingTrial_ShouldFail()
    {
        // Arrange
        var trial = await CreateTestTrial();

        // Act
        var response = await Client.GetAsync(Routes.GetVersionedRoute($"{Routes.Trials}/test", 1));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetTrials_WithStatusFilter_ShouldReturnFilteredTrials()
    {
        // Arrange
        await CreateTestTrials();

        // Act
        var response = await Client.GetAsync(Routes.GetVersionedRoute($"{Routes.Trials}?status=Ongoing", 1));
        var results = await response.Content.ReadFromJsonAsync<IEnumerable<ClinicalTrialDto>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        results.Should().NotBeNull();
        results!.Should().AllSatisfy(trial => trial.Status.Should().Be(TrialStatus.Ongoing));
    }

    private async Task<ClinicalTrial> CreateTestTrial()
    {
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var trial = new ClinicalTrial
        {
            TrialId = "TEST001",
            Title = "Test Trial",
            StartDate = DateTime.UtcNow,
            Status = TrialStatus.Ongoing,
            DurationInDays = 30
        };

        context.ClinicalTrials.Add(trial);
        await context.SaveChangesAsync();

        return trial;
    }

    private async Task CreateTestTrials()
    {
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var trials = new[]
        {
            new ClinicalTrial
            {
                TrialId = "TEST001",
                Title = "Ongoing Trial",
                StartDate = DateTime.UtcNow,
                Status = TrialStatus.Ongoing,
                DurationInDays = 30
            },
            new ClinicalTrial
            {
                TrialId = "TEST002",
                Title = "Completed Trial",
                StartDate = DateTime.UtcNow.AddMonths(-1),
                EndDate = DateTime.UtcNow,
                Status = TrialStatus.Completed,
                DurationInDays = 30
            }
        };

        context.ClinicalTrials.AddRange(trials);
        await context.SaveChangesAsync();
    }
} 