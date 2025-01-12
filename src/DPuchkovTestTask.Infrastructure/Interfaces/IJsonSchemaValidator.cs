namespace DPuchkovTestTask.Infrastructure.Interfaces;

public interface IJsonSchemaValidator
{
    Task<(bool IsValid, IEnumerable<string> Errors)> ValidateAsync(string json, CancellationToken cancellationToken);
} 