using MediatR;

namespace Shared.Implementations.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}