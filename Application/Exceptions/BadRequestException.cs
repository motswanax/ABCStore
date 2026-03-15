using System.Net;

namespace Application.Exceptions;
public class BadRequestException : Exception
{
    public BadRequestException(List<string> errorMessages, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        ErrorMessages = errorMessages;
        StatusCode = statusCode;
    }

    public List<string> ErrorMessages { get; set; } = [];
    public HttpStatusCode StatusCode { get; set; }
}
