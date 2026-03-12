namespace HighHeavyShipment.Domain;

public abstract record Result
{
    public sealed record Success : Result;
    public sealed record Failure(string Code, string Message) : Result;

    public bool IsSuccess => this is Success;
    public bool IsFailure => this is Failure;

    public TResult Match<TResult>(
        Func<Success, TResult> onSuccess,
        Func<Failure, TResult> onFailure)
        => this switch
        {
            Success success => onSuccess(success),
            Failure failure => onFailure(failure),
            _ => throw new InvalidOperationException("Unknown result type.")
        };
}

public abstract record Result<T>
{
    public sealed record Success(T Value) : Result<T>;
    public sealed record Failure(string Code, string Message) : Result<T>;

    public bool IsSuccess => this is Success;
    public bool IsFailure => this is Failure;

    public TResult Match<TResult>(
        Func<Success, TResult> onSuccess,
        Func<Failure, TResult> onFailure)
        => this switch
        {
            Success success => onSuccess(success),
            Failure failure => onFailure(failure),
            _ => throw new InvalidOperationException("Unknown result type.")
        };
}
