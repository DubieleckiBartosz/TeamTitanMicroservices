using Shared.Domain.Abstractions; 

namespace Shared.Implementations.Projection;

public interface IProjection
{
    Type[] Handles { get; }
    Task Handle(IEvent @event, CancellationToken cancellationToken);
}