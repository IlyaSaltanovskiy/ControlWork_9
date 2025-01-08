namespace ControlWork_9.Response;

public class BaseApiResponse<T>
{
    public T Data { get; }
    public string? Message { get; }
    
    public BaseApiResponse(T data, string? message = null)
    {
        Data = data;
        Message = message;
    }
   
}