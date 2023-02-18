using MediatR;
using Shared.Domain.Abstractions;

namespace Shared.Implementations.Decorators;

public class MediatRDecorator : IDomainDecorator
{
    private readonly IMediator _mediator;

    public MediatRDecorator(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Publish<TNotification>(TNotification notification,
        CancellationToken cancellationToken = default(CancellationToken)) where TNotification : IDomainNotification
    {
        var wrapper = this.CreateNotification(notification);
        await this._mediator.Publish(wrapper, cancellationToken);
    }

    private INotification CreateNotification(IDomainNotification domainNotification)
    {
        var genericDispatcherType = typeof(DomainNotification<>).MakeGenericType(domainNotification.GetType());  
        return (INotification)Activator.CreateInstance(genericDispatcherType, domainNotification);
    }
}