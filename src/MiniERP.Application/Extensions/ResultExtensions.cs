using FluentResults;

namespace MiniERP.Application.Extensions;

public static class ResultExtensions
{
    public static bool HasErrorCode(this ResultBase result, string errorCode)
    {
        return result.Errors.Any(e =>
            e.Metadata.TryGetValue("ErrorCode", out var code) &&
            code?.ToString() == errorCode);
    }
}