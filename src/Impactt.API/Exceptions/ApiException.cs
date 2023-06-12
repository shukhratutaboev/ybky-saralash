namespace Impactt.API.Exceptions;

public class ApiException : Exception
{
    public int StatusCode { get; set; }
    public ApiException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}