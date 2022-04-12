using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PPMRm.ARTMIS.PeriodShipments;
using PPMRm.Items;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace PPMRm
{
    [DependsOn(
        typeof(PPMRmDomainModule)
    )]
    public class PPMRmMartenModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<PPMRmMartenModule>();
            });
            //Register IDocumentStore instance as singleton
            context.Services.AddSingleton<IDocumentStore>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                //var environment = sp.GetRequiredService<IHostEnvironment>();

                var connectionString = configuration.GetConnectionString("Default");
                return new DocumentStore(new PPMRmStoreOptions(connectionString, null));
            });

            // Register IDocumentSession and IQuerySession as scoped
            context.Services.AddScoped(sp => sp.GetRequiredService<IDocumentStore>().LightweightSession());
            context.Services.AddScoped(sp => sp.GetRequiredService<IDocumentStore>().QuerySession());
            context.Services.AddScoped<IItemRepository, ItemRepository>();
            context.Services.AddScoped<IPeriodShipmentRepository, PeriodShipmentRepository>();

        }
    }
}
