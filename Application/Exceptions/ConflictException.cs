using System.Net;

namespace Application.Exceptions;
public class ConflictException : Exception
{
    public ConflictException(List<string> errorMessages, HttpStatusCode statusCode = HttpStatusCode.Conflict)
    {
        ErrorMessages = errorMessages;
        StatusCode = statusCode;
    }

    public List<string> ErrorMessages { get; set; } = [];
    public HttpStatusCode StatusCode { get; set; }
}
