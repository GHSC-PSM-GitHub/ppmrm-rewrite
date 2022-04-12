using Marten;
using Marten.Events;
using Marten.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMRm.ARTMIS.PeriodShipments
{
    public class PeriodShipmentRepository : IPeriodShipmentRepository
    {
        IDocumentSession Session { get; }
        public PeriodShipmentRepository(IDocumentSession documentSession)
        {
            Session = documentSession;
        }
        public async Task<PeriodShipment> GetAsync(string countryId, int periodId)
        {
            var periodShipmentId = $"{countryId}-{periodId}";
            var periodShipment = await Session.LoadAsync<PeriodShipment>(periodShipmentId);
            if (periodShipment != null) return periodShipment;
            periodShipment = new PeriodShipment { Id = periodShipmentId, CountryId = countryId, PeriodId = periodId };
            var events = await Session.Events.QueryAllRawEvents()
                .OrderBy(e => e.Sequence).ToListAsync();
            //.Where(e => e.DotNetTypeName == typeof(OrderLineInserted).AssemblyQualifiedName || e.DotNetTypeName == typeof(OrderLineUpdated).AssemblyQualifiedName || e.DotNetTypeName == typeof(OrderLineDeleted).AssemblyQualifiedName)
            //.Where(e => e.CountryId == countryId && e.PeriodId <= periodId)
            //.ToListAsync();
            foreach (var @event in events.Select(e => e.Data as OrderLineEvent).Where(e => e.CountryId == countryId && e.PeriodId <= periodId && ARTMISConsts.PPMRmProductMappings.ContainsKey(e.ProductId)))
            {
                var e = (OrderLineEvent)@event;
                if (e is OrderLineInserted) periodShipment.Apply((OrderLineInserted)e);
                else if (e is OrderLineUpdated) periodShipment.Apply((OrderLineUpdated)e);
                else if (e is OrderLineDeleted) periodShipment.Apply((OrderLineDeleted)e);
            }
            Session.Store<PeriodShipment>(periodShipment);
            await Session.SaveChangesAsync();
            return periodShipment;
        }
    }
}
