using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PPMRm.Data;
using Volo.Abp.DependencyInjection;

namespace PPMRm.EntityFrameworkCore
{
    public class EntityFrameworkCorePPMRmDbSchemaMigrator
        : IPPMRmDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCorePPMRmDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the PPMRmDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<PPMRmDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}
