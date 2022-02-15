using System;
using System.Threading.Tasks;
using Marten;
using Marten.Schema;

namespace PPMRm
{
    public class InitialData : IInitialData
    {
        private readonly object[] _initialData;

        public InitialData(params object[] initialData)
        {
            _initialData = initialData;
        }

        public async Task Populate(IDocumentStore store)
        {
            using var session = store.LightweightSession();
            // Marten UPSERT will cater for existing records
            session.Store(_initialData);
            await session.SaveChangesAsync();
        }
    }
}
