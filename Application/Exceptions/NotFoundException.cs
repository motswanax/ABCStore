using System.Net;

namespace Application.Exceptions;
public class NotFoundException : Exception
{
    public NotFoundException(List<string> errorMessages, HttpStatusCode statusCode = HttpStatusCode.NotFound)
    {
        ErrorMessages = errorMessages;
        StatusCode = statusCode;
    }

    public List<string> ErrorMessages { get; set; } = [];
    public HttpStatusCode StatusCode { get; set; }
}
