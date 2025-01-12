using MediatR;

namespace DPuchkovTestTask.Application.Files.Commands.UploadFile;

public record UploadFileCommand(
    Stream FileStream,
    string FileName,
    long FileSize) : IRequest<UploadFileResult>;

public record UploadFileResult(bool IsSuccess, string Message); 