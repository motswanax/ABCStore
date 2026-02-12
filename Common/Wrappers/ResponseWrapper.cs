namespace Common.Wrappers;

public class ResponseWrapper<T>
{
    public T Data { get; set; } = default!;
    public bool IsSuccessful { get; set; }
    public string? Message { get; set; }

    public ResponseWrapper<T> Success(T data, string? message = null)
    {
        IsSuccessful = true;
        Message = message;
        Data = data;
        return this;
    }

    public ResponseWrapper<T> Fail(string message)
    {
        IsSuccessful = false;
        Message = message;
        return this;
    }
}