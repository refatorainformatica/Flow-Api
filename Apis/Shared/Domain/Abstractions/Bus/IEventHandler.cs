using System.Collections.Generic;
using Shared.Domain.Abstractions.Events;

namespace Shared.Domain.Abstractions.Bus
{
    public interface IEventHandler
    {
        void RaiseEvent(IEvent @event);

        void RaiseEvents(IList<IEvent> events);
    }
}
