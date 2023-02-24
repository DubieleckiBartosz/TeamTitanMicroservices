namespace Identity.Application.Wrappers;

public class Response<T>
{
    public bool Success { get; }
    public string? Message { get; set; }

    public T Data { get; }

    private Response(T data, string? message)
    {
        Data = data;
        Message = message;
        Success = true;
    }

    public static Response<T> Ok(T data)
    {
        return new Response<T>(data, null);
    }

    public static Response<T> Ok(T data, string? message)
    {
        return new Response<T>(data, message);
    }
}