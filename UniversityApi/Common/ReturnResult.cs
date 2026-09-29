namespace UniversityApi.Common;

public class ReturnResult<T>
{
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public T? Result { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? TraceId { get; set; }

    public static ReturnResult<T> Success(
        T result,
        int statusCode = 200)
    {
        return new ReturnResult<T>
        {
            StatusCode = statusCode,
            IsSuccess = true,
            Result = result
        };
    }

    public static ReturnResult<T> Error(
        int statusCode,
        string errorCode,
        string errorMessage)
    {
        return new ReturnResult<T>
        {
            StatusCode = statusCode,
            IsSuccess = false,
            ErrorCode = errorCode,
            ErrorMessage = errorMessage
        };
    }
}