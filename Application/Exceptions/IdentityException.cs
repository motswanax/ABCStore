using System.Net;

namespace Application.Exceptions;
public class IdentityException : Exception
{
    public IdentityException(List<string> errorMessages, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        ErrorMessages = errorMessages;
        StatusCode = statusCode;
    }

    public List<string> ErrorMessages { get; set; } = [];
    public HttpStatusCode StatusCode { get; set; }
}
