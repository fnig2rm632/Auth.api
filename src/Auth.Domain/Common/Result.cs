namespace Auth.Domain.Common;

public class Result
{
    public bool IsSuccess { get;}
    public Error Error { get;}
    
    private Result()
    {
        IsSuccess = true;
        Error = Error.None;
    }
    
    private Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }

    public static Result Success() => new();
    private static Result Failure(Error error) => new(error);
    
    public static implicit operator Result(Error error) => Failure(error);
}

public class Result<T>
{
    public bool IsSuccess { get;}
    public T? Value { get;}
    public Error Error { get;}

    private Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }
    
    private Result(T? value)
    {
        IsSuccess = true;
        Value = value;
        Error = Error.None;
    }

    private static Result<T> Success(T value) => new(value);
    private static Result<T> Failure(Error error) => new(error);
    
    public static implicit operator Result<T>(Error error) => Failure(error);
    public static implicit operator Result<T>(T value) => Success(value);
    
}