using System.Net;

namespace Shared.Implementations.Core.Exceptions;

public class EventException : TeamTitanApplicationException
{
    public EventException(string messageDetail, string title,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(messageDetail,
        title, statusCode)
    {
    }
}