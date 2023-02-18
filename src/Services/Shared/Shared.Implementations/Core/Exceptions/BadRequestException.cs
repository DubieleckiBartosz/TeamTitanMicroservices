using System.Net;

namespace Shared.Implementations.Core.Exceptions;

public class BadRequestException : TeamTitanApplicationException
{
    public BadRequestException(string messageDetail, string title) : base(messageDetail,
        title, HttpStatusCode.BadRequest)
    {
    }
}