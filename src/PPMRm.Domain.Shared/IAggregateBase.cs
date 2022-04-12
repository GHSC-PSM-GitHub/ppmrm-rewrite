using System;
using System.Collections.Generic;

namespace PPMRm
{
    public interface IAggregateBase
    {
        void ClearUncommittedEvents();
        IEnumerable<object> GetUncommittedEvents();
        string StreamId { get; }
    }
}
