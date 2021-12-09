using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace PPMRm.Data
{
    /* This is used if database provider does't define
     * IPPMRmDbSchemaMigrator implementation.
     */
    public class NullPPMRmDbSchemaMigrator : IPPMRmDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}