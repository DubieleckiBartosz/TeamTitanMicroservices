using System.Net;

namespace Shared.Implementations.Core.Exceptions;

public class DatabaseException : TeamTitanApplicationException
{
    public DatabaseException(string messageDetail, string title, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(messageDetail,
        title, statusCode)
    {
    }
}