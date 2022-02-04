using Marten;
using Marten.Events.Projections;
using Microsoft.Extensions.Hosting;
using Weasel.Postgresql;

namespace PPMRm
{
    public class PPMRmStoreOptions : StoreOptions
    {   
        public PPMRmStoreOptions(string connectionString, IHostEnvironment environment)
        {
            Connection(connectionString);

            // AutoCreate Schema
            if(!environment.IsProduction())
                AutoCreateSchemaObjects = AutoCreate.All;

            // Add Schema Registries
            Schema.Include<PPMRmMartenRegistry>();


            // Run the Order as an inline projection
            Projections.SelfAggregate<ARTMIS.Orders.Order>(ProjectionLifecycle.Inline);
            // Add InitialData Seed
            // InitialData.Add(new InitialData(InitialDatasets.Products));
        }
    }
}
