namespace App.Core.Results;

public class Result<T>
{
    public bool IsSuccessful { get; private set; }
    public string Message { get; private set; }
    public T Data { get; private set; }

    private Result(bool isSuccessful, string message, T data)
    {
        IsSuccessful = isSuccessful;
        Message = message;
        Data = data;
    }
    
    public static Result<T> Success(T data, string message = "Operation successful.")
    {
        return new Result<T>(true, message, data);
    }
    
    public static Result<T> Failure(string message, T data = default)
    {
        return new Result<T>(false, message, data);
    }
}