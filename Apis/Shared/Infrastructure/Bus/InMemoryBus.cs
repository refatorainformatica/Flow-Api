using System.Collections.Generic;
using System.Linq;
using MediatR;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Events;

namespace Shared.Infrastructure.Bus
{
    public class InMemoryBus : IMemoryBus
    {
        private readonly IMediator _mediator;

        public InMemoryBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void RaiseEvent(IEvent @event)
        {
            _mediator.Publish(@event);
        }

        public void RaiseEvents(IList<IEvent> events)
        {
            events.ToList().ForEach(e => _mediator.Publish(e));
        }

        public void SendCommand<T>(T command)
            where T : IRequest
        {
            _mediator.Send(command);
        }
    }
}
