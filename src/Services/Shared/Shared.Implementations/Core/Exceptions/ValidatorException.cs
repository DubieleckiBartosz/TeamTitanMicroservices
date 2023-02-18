using FluentValidation.Results;

namespace Shared.Implementations.Core.Exceptions;

public class ValidatorException : Exception
{
    public List<string> Errors { get; }

    public ValidatorException() : base("One or more validation failures have occurred.")
    {
        Errors = new List<string>();
    }

    public ValidatorException(IEnumerable<ValidationFailure> failures) : this()
    {
        foreach (var itemFailure in failures)
        {
            Errors.Add(itemFailure.ErrorMessage);
        }
    }
}