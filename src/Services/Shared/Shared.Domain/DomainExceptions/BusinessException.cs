using System.Net;

namespace Shared.Domain.DomainExceptions;

public class BusinessException : Exception
{
    public string Title { get; }
    public HttpStatusCode StatusCode { get; }

    public BusinessException(string title, string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) :
        base(message)
    {
        Title = title;
        StatusCode = statusCode;
    }
}