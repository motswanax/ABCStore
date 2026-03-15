using System.Net;

namespace Application.Exceptions;
public class ForbiddenException : Exception
{
    public ForbiddenException(List<string> errorMessages, HttpStatusCode statusCode = HttpStatusCode.Forbidden)
    {
        ErrorMessages = errorMessages;
        StatusCode = statusCode;
    }

    public List<string> ErrorMessages { get; set; } = [];
    public HttpStatusCode StatusCode { get; set; }
}
