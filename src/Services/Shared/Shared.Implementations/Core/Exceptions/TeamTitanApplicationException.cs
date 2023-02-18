using System.Net;

namespace Shared.Implementations.Core.Exceptions;

public class TeamTitanApplicationException : Exception
{
    public string Title { get; }
    public HttpStatusCode StatusCode { get; }


    public TeamTitanApplicationException(string messageDetail, string title, HttpStatusCode statusCode) : base(
        messageDetail)
    {
        Title = title;
        StatusCode = statusCode;
    }
}