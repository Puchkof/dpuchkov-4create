using FluentValidation;
using Microsoft.Extensions.Options;

namespace DPuchkovTestTask.Application.Files.Commands.UploadFile;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator(IOptions<Options.FileOptions> fileOptions)
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .Must(fileName => Path.GetExtension(fileName).Equals(".json", StringComparison.InvariantCultureIgnoreCase))
            .WithMessage("Only .json files are allowed.");

        RuleFor(x => x.FileSize)
            .Must(size => size > 0)
            .WithMessage("File size must be greater than 0.")
            .Must(size => size <= fileOptions.Value.SizeLimitMb * 1024 * 1024)
            .WithMessage(x => $"File size exceeds the {fileOptions.Value.SizeLimitMb}MB limit.");

        RuleFor(x => x.FileStream)
            .NotNull()
            .WithMessage("File stream cannot be null.");
    }
} 