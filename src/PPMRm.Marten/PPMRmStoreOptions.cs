using Marten;
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

            // Add InitialData Seed
            // InitialData.Add(new InitialData(InitialDatasets.Products));
        }
    }
}
