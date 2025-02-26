using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Aggregates;
using Core.Events;
using Core.Repositories;
using Marten;

namespace Core.Marten.Repository
{

    public class MartenRepository<T> : IRepository<T> where T : class, IAggregate
    {
        private readonly IDocumentSession documentSession;
        private readonly IEventBus eventBus;

        public MartenRepository(
            IDocumentSession documentSession,
            IEventBus eventBus
        )
        {
            this.documentSession = documentSession ?? throw new ArgumentNullException(nameof(documentSession));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public Task<T?> Find(Guid id, CancellationToken cancellationToken)
        {
            return documentSession.Events.AggregateStreamAsync<T>(id, token: cancellationToken);
        }

        public Task Add(T aggregate, CancellationToken cancellationToken)
        {
            return Store(aggregate, cancellationToken);
        }

        public Task Update(T aggregate, CancellationToken cancellationToken)
        {
            return Store(aggregate, cancellationToken);
        }

        public Task Delete(T aggregate, CancellationToken cancellationToken)
        {
            return Store(aggregate, cancellationToken);
        }

        private async Task Store(T aggregate, CancellationToken cancellationToken)
        {
            var events = aggregate.DequeueUncommittedEvents();
            documentSession.Events.Append(
                aggregate.Id,
                events
            );
            await documentSession.SaveChangesAsync(cancellationToken);
            await eventBus.Publish(events);
        }
    }
}