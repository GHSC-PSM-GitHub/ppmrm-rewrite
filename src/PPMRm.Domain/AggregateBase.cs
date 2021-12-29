using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Volo.Abp.Domain.Entities;

namespace PPMRm
{
    public abstract class AggregateBase : AggregateBase<string>
    {
        public override string StreamId => Id;
    }

    public abstract class AggregateBase<TKey> : Entity<TKey>, IAggregateBase
    {
        // For protecting the state, i.e. conflict prevention
        // The setter is only public for setting up test conditions
        public long Version { get; set; }

        public virtual string StreamId => Id.ToString();

        // JsonIgnore - for making sure that it won't be stored in inline projection
        [JsonIgnore]
        private readonly List<object> _uncommittedEvents = new List<object>();

        // Get the deltas, i.e. events that make up the state, not yet persisted
        public IEnumerable<object> GetUncommittedEvents()
        {
            return _uncommittedEvents;
        }

        // Mark the deltas as persisted.
        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        protected void AddUncommittedEvent(object @event)
        {
            // add the event to the uncommitted list
            _uncommittedEvents.Add(@event);
        }
    }
}
