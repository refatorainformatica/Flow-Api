using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Abstractions.Events;

namespace Shared.Domain.Abstractions.Bus
{
    public abstract class CommandHandler
    {
        private readonly IMediator _mediator;
        private readonly DbContext _dbContext;

        protected CommandHandler(DbContext dbContext, IMediator mediator)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task ExecuteTransactionAsync(
            Func<Task> action,
            IList<DomainEvent> events,
            CancellationToken cancellationToken
        )
        {
            await action();
            await _dbContext.SaveChangesAsync(cancellationToken);
            PublishEvents(events, cancellationToken);
        }

        public async Task ExecuteTransactionAsync(
            Action action,
            IList<DomainEvent> events,
            CancellationToken cancellationToken
        )
        {
            action();
            await _dbContext.SaveChangesAsync(cancellationToken);
            PublishEvents(events, cancellationToken);
        }

        public void PublishEvents(IList<DomainEvent> events, CancellationToken cancellationToken)
        {
            events
                .ToList()
                .ForEach(
                    async (eventItem) =>
                    {
                        await _mediator.Publish(eventItem, cancellationToken);
                    }
                );

            events.Clear();
        }
    }
}
