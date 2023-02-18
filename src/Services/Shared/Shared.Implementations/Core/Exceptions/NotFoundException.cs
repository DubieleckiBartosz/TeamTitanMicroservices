using System.Net;

namespace Shared.Implementations.Core.Exceptions;

public class NotFoundException : TeamTitanApplicationException
{
    public NotFoundException(string messageDetail, string title) : base(messageDetail, title, HttpStatusCode.NotFound)
    {
    }
}