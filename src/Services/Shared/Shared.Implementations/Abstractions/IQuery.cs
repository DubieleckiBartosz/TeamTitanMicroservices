using MediatR;

namespace Shared.Implementations.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}