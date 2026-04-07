namespace ResultPattern.Sample.Common;

public static class ResultExtensions
{
    public static Result<TResult> Map<TValue, TResult>(this Result<TValue> result, Func<TValue, TResult> map)
    {
        return result.IsSuccess && result.Value is not null
            ? Result<TResult>.Success(map(result.Value))
            : Result<TResult>.Failure(result.Error ?? "Unknown error.", result.ErrorCode ?? "unknown_error");
    }

    public static Result<TResult> Bind<TValue, TResult>(this Result<TValue> result, Func<TValue, Result<TResult>> bind)
    {
        return result.IsSuccess && result.Value is not null
            ? bind(result.Value)
            : Result<TResult>.Failure(result.Error ?? "Unknown error.", result.ErrorCode ?? "unknown_error");
    }
}
