namespace Shared.Domain.Abstractions;

public interface IDomainDecorator
{
    Task Publish<TNotification>(TNotification notification,
        CancellationToken cancellationToken = default(CancellationToken))
        where TNotification : IDomainNotification;
}