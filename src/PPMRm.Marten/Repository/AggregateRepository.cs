using System.Linq;
using Marten;
using Volo.Abp.Domain.Services;
using System;
using System.Threading.Tasks;

namespace PPMRm.Repository
{
    public sealed class AggregateRepository : DomainService
    {
        private readonly IDocumentStore store;

        public AggregateRepository(IDocumentStore store)
        {
            this.store = store;
        }

        public async Task StoreAsync(AggregateBase aggregate)
        {
            using (var session = store.OpenSession())
            {
                // Take non-persisted events, push them to the event stream, indexed by the aggregate ID
                var events = aggregate.GetUncommittedEvents().ToArray();
                session.Events.Append(aggregate.Id, aggregate.Version, events);
                await session.SaveChangesAsync();
            }
            aggregate.ClearUncommittedEvents();
        }

        public async Task<T> LoadAsync<T>(string id, int? version = null) where T : AggregateBase
        {
            using var session = store.LightweightSession();
            var aggregate = await session.Events.AggregateStreamAsync<T>(id, version ?? 0);
            return aggregate ?? throw new InvalidOperationException($"No aggregate by id {id}.");
        }
    }

}
