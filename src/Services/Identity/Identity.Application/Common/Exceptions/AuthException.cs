using System.Net;

namespace Identity.Application.Common.Exceptions;

public class AuthException : Exception
{
    public int Code { get; set; }
    public string Title { get; set; }

    public AuthException(string error, string title, HttpStatusCode code = HttpStatusCode.BadRequest) : base(error)
    {
        Title = title;
        Code = (int)code;
    }
}