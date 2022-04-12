using PPMRm.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace PPMRm.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(PPMRmEntityFrameworkCoreModule),
        typeof(PPMRmApplicationContractsModule),
        typeof(PPMRmMartenModule)
        )]
    public class PPMRmDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
