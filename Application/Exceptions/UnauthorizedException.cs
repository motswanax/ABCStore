using System.Net;

namespace Application.Exceptions;
public class UnauthorizedException : Exception
{
    public UnauthorizedException(List<string> errorMessages, HttpStatusCode statusCode = HttpStatusCode.Unauthorized)
    {
        ErrorMessages = errorMessages;
        StatusCode = statusCode;
    }

    public List<string> ErrorMessages { get; set; } = [];
    public HttpStatusCode StatusCode { get; set; }
}
