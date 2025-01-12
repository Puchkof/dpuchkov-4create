using System.Net;
using System.Net.Http.Headers;
using DPuchkovTestTask.API.EndpointRegistrations;
using DPuchkovTestTask.Domain.Enums;
using DPuchkovTestTask.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DPuchkovTestTask.IntegrationTests.ClinicalTrials;

public class UploadFileTests : IntegrationTestBase
{
    [Fact]
    public async Task Upload_ValidFile_ShouldSucceed()
    {
        // Arrange
        var jsonContent = """
        [
            {
                "trialId": "TEST001",
                "title": "Test Trial",
                "startDate": "2024-01-01",
                "endDate": "2024-02-01",
                "participants": 100,
                "status": "Ongoing"
            }
        ]
        """;

        using var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(jsonContent));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        content.Add(fileContent, "file", "trial.json");

        // Act
        var response = await Client.PostAsync(Routes.GetVersionedRoute(Routes.UploadFile, 1), content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify database
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var trial = await context.ClinicalTrials.FirstOrDefaultAsync(t => t.TrialId == "TEST001");
        
        trial.Should().NotBeNull();
        trial!.Title.Should().Be("Test Trial");
        trial.Status.Should().Be(TrialStatus.Ongoing);
        trial.Participants.Should().Be(100);
        trial.StartDate.Should().Be(new DateTime(2024, 1, 1));
        trial.EndDate.Should().Be(new DateTime(2024, 2, 1));
    }

    [Fact]
    public async Task Upload_InvalidJson_ShouldFail()
    {
        // Arrange
        var invalidJson = """
        [
            {
                "trialId": "TEST001",
                "title": "Test Trial",
                "startDate": "invalid-date",
                "status": "InvalidStatus"
            }
        ]
        """;

        using var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(invalidJson));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        content.Add(fileContent, "file", "invalid.json");

        // Act
        var response = await Client.PostAsync(Routes.GetVersionedRoute(Routes.UploadFile, 1), content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Upload_InvalidExtension_ShouldFail()
    {
        // Arrange
        var invalidJson = """
                          [
                              {
                                  "trialId": "TEST001",
                                  "title": "Test Trial",
                                  "startDate": "invalid-date",
                                  "status": "InvalidStatus"
                              }
                          ]
                          """;

        using var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(invalidJson));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/plain");
        content.Add(fileContent, "file", "invalid.txt");

        // Act
        var response = await Client.PostAsync(Routes.GetVersionedRoute(Routes.UploadFile, 1), content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Upload_EmptyFile_ShouldFail()
    {
        // Arrange
        using var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent([]);
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        content.Add(fileContent, "file", "empty.json");

        // Act
        var response = await Client.PostAsync(Routes.GetVersionedRoute(Routes.UploadFile, 1), content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
} 