using Marten;
using Marten.Events.Projections;
using Microsoft.Extensions.Hosting;
using PPMRm.Projections;
using Weasel.Postgresql;

namespace PPMRm
{
    public class PPMRmStoreOptions : StoreOptions
    {   
        public PPMRmStoreOptions(string connectionString, IHostEnvironment environment)
        {
            Connection(connectionString);

            // AutoCreate Schema
            if(environment != null && !environment.IsProduction())
                AutoCreateSchemaObjects = AutoCreate.All;

            // Add Schema Registries
            Schema.Include<PPMRmMartenRegistry>();
            Schema.For<Items.Item>().Index(x => x.Name);
            Schema.For<ARTMIS.OrderLines.OrderLine>().Duplicate(o => o.ProductId);
            Schema.For<ARTMIS.OrderLines.OrderLine>().Duplicate(o => o.CountryId);
            Schema.For<ARTMIS.OrderLines.OrderLine>().Duplicate(o => o.OrderNumber);
            Schema.For<ARTMIS.OrderLines.OrderLine>().Duplicate(o => o.OrderLineNumber);
            Schema.For<ARTMIS.OrderLines.OrderLine>().Index(o => new { o.OrderNumber, o.OrderLineNumber });

            Projections.Add(new OrderLineProjection(), ProjectionLifecycle.Inline);
            Events.StreamIdentity = Marten.Events.StreamIdentity.AsString;
            // Run the Order as an inline projection
            //Projections.SelfAggregate<ARTMIS.Orders.Order>(ProjectionLifecycle.Inline);
            // Add InitialData Seed
            // InitialData.Add(new InitialData(InitialDatasets.Products));
        }
    }
}
