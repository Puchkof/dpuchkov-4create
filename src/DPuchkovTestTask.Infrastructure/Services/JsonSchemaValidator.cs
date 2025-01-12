using DPuchkovTestTask.Infrastructure.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace DPuchkovTestTask.Infrastructure.Services;

public class JsonSchemaValidator : IJsonSchemaValidator
{
    private readonly JSchema _schema;

    public JsonSchemaValidator()
    {
        using var stream = typeof(JsonSchemaValidator).Assembly
            .GetManifestResourceStream("DPuchkovTestTask.Infrastructure.Schemas.clinical-trials-schema.json");
        using var reader = new StreamReader(stream!);
        var schemaJson = reader.ReadToEnd();
        _schema = JSchema.Parse(schemaJson);
    }

    public async Task<(bool IsValid, IEnumerable<string> Errors)> ValidateAsync(string json, CancellationToken cancellationToken)
    {
        try
        {
            var jsonObject = JToken.Parse(json);
            var isValid = jsonObject.IsValid(_schema, out IList<string> errorMessages);

            return await Task.FromResult((isValid, errorMessages));
        }
        catch (Exception ex)
        {
            return await Task.FromResult((false, new[] { $"JSON parsing error: {ex.Message}" }));
        }
    }
} 