using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PPMRm.EntityFrameworkCore;
using PPMRm.Localization;
using PPMRm.MultiTenancy;
using PPMRm.Web.Menus;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.Web;
using Volo.Abp.AspNetCore.Mvc.UI.Components.LayoutHook;
using PPMRm.Web.Components.Footer;
using Volo.Abp.BackgroundWorkers;
using Microsoft.AspNetCore.HttpOverrides;
using Hangfire;
using Hangfire.PostgreSql;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Logging;
using PPMRm.Web.Jobs;
using Volo.Abp.Emailing;
using Volo.Abp.TextTemplating;
using Serilog.Core;
using static Volo.Abp.AspNetCore.Mvc.UI.Components.LayoutHook.LayoutHooks;
using System.Threading.Tasks;
using static IdentityServer4.Models.IdentityResources;
using Volo.Abp.Emailing.Templates;
using Hangfire.Dashboard;
using PPMRm.Core;
using PPMRm.Items;
using PPMRm.PeriodReports;
using PPMRm.Products;
using PPMRm.ARTMIS.PeriodShipments;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using PPMRm.ARTMIS;
using System.Security.Cryptography;
using Volo.Abp.Uow;
using Volo.CmsKit.Tags;

namespace PPMRm.Web
{
    [DependsOn(
        typeof(PPMRmHttpApiModule),
        typeof(PPMRmApplicationModule),
        typeof(PPMRmEntityFrameworkCoreModule),
        typeof(AbpAutofacModule),
        typeof(AbpIdentityWebModule),
        typeof(AbpSettingManagementWebModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpTenantManagementWebModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule)
        )]
    [DependsOn(typeof(CmsKitWebModule))]
    [DependsOn(typeof(AbpBackgroundWorkersModule))]
    [DependsOn(
    //...other dependencies
    typeof(AbpBackgroundJobsHangfireModule) //Add the new module dependency
    )]
    public class PPMRmWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(PPMRmResource),
                    typeof(PPMRmDomainModule).Assembly,
                    typeof(PPMRmDomainSharedModule).Assembly,
                    typeof(PPMRmApplicationModule).Assembly,
                    typeof(PPMRmApplicationContractsModule).Assembly,
                    typeof(PPMRmWebModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();
            context.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.ForwardLimit = 2;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
            ConfigureUrls(configuration);
            ConfigureBundles();
            ConfigureAuthentication(context, configuration);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureLocalizationServices();
            ConfigureNavigationServices();
            ConfigureAutoApiControllers();
            ConfigureSwaggerServices(context.Services);
            ConfigureLayouts();
            ConfigureHangfire(context, configuration);

        }

        private void ConfigureHangfire(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddTransient<ISyncManager, SyncManager>();
            context.Services.AddHangfire(config =>
            {
                config.UsePostgreSqlStorage(configuration.GetConnectionString("Default"));
            });
        }
        private void ConfigureLayouts()
        {
            Configure<AbpLayoutHookOptions>(options =>
            {
                options.Add(
                    LayoutHooks.Body.Last, //The hook name
                    typeof(FooterViewComponent) //The component to add
                );
            });
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureBundles()
        {
            Configure<AbpBundlingOptions>(options =>
            {
                options.StyleBundles.Configure(
                    BasicThemeBundles.Styles.Global,
                    bundle =>
                    {
                        bundle.AddFiles("/global-styles.css");
                        bundle.AddFiles("/libs/bootstrap/css/bootstrap.css");
                        bundle.AddFiles("/styles/bootstrap-multiselect.css");
                    }
                );
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = "PPMRm";
                });
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<PPMRmWebModule>();
            });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<PPMRmDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}PPMRm.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PPMRmDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}PPMRm.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PPMRmApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}PPMRm.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PPMRmApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}PPMRm.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PPMRmWebModule>(hostingEnvironment.ContentRootPath);
                });
            }
        }

        private void ConfigureLocalizationServices()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                //options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
                //options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                //options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
                //options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
                //options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish"));
                //options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
                //options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi", "in"));
                //options.Languages.Add(new LanguageInfo("it", "it", "Italian", "it"));
                //options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
                //options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
                //options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak"));
                //options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                //options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                //options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
                //options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch", "de"));
                //options.Languages.Add(new LanguageInfo("es", "es", "Español"));
            });
        }

        private void ConfigureNavigationServices()
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new PPMRmMenuContributor());
            });
        }

        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(PPMRmApplicationModule).Assembly);
            });
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddAbpSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PPMRm API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            // Init background worker
            //context.AddBackgroundWorker<PPMRm.Web.Jobs.BackgroundSyncWorker>();
            
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            app.UseForwardedHeaders();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseUnitOfWork();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "PPMRm API");
            });
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                DashboardTitle = "PPMRm Jobs",
                Authorization = new[]
                {
                    new HangfireAuthorizationFilter(),
                },

            });
            app.UseConfiguredEndpoints();

            RecurringJob.AddOrUpdate<ISyncManager>(x => x.Start(), Cron.Monthly(2, 6));
        }
    }
}

public interface ISyncManager
{
    Task Start();
}
public class SyncManager : ISyncManager
{
    IEmailSender _emailSender;
    ITemplateRenderer _templateRenderer;
    private IServiceProvider _serviceProvider;

    IRepository<PeriodReport, string> Repository { get; set; }
    PeriodReportManager PeriodReportManager { get; set; }
    IPeriodShipmentRepository ShipmentRepository { get; set; }
    IRepository<Country, string> CountryRepository { get; set; }
    IRepository<Period, int> PeriodRepository { get; set; }
    IRepository<Product, string> ProductRepository { get; set; }
    IItemRepository ItemRepository { get; set; }

    public SyncManager(IEmailSender emailSender, ITemplateRenderer templateRenderer, IRepository<PeriodReport, string> repository, IRepository<Country, string> countryRepository, IRepository<Product, string> productRepository, IRepository<Period, int> periodRepository, IItemRepository itemRepository, PeriodReportManager periodReportManager, IPeriodShipmentRepository shipmentRepository)
    {
        _emailSender = emailSender;
        _templateRenderer = templateRenderer;
        Repository = repository;
        PeriodRepository = periodRepository;
        CountryRepository = countryRepository;
        PeriodReportManager = periodReportManager;
        ItemRepository = itemRepository;
        ShipmentRepository = shipmentRepository;
        ProductRepository = productRepository;
    }

    [UnitOfWork]
    public async Task Start()
    {
        var lastPeriod = await PeriodReportManager.GetCurrentPeriodAsync();
        var nextPeriod = await PeriodReportManager.GetNextPeriodAsync();
        var nextPeriodId = nextPeriod.Id;
        var items = await ItemRepository.GetListAsync();
        var products = await ProductRepository.ToListAsync();
        var countries = await CountryRepository.ToListAsync();
        var period = nextPeriod;
        var reports = await PeriodReportManager.CreateManyAsync(period, countries);
        var lastPeriodReports = await PeriodReportManager.GetPeriodReportsAsync(lastPeriod.Id);
        //await Repository.InsertManyAsync(reports);
        foreach (var report in reports)
        {
            report.Open();
            var shipment = await ShipmentRepository.GetAsync(report.CountryId, report.PeriodId);
            var periodShipments = shipment.Shipments
                .Where(l => l.PPMRmProductId != null && l.ShipmentDateType != ARTMISConsts.OrderDeliveryDateTypes.ActualDeliveryDate || (l.ShipmentDate >= period.StartDate));

            foreach (var s in periodShipments)
            {
                var shipmentItem = items.SingleOrDefault(i => i.Id == s.ProductId);
                var product = products.SingleOrDefault(p => p.Id == shipmentItem?.ProductId);
                if (product == null)
                {
                    Console.WriteLine($"{s.ProductId} - {s.PPMRmProductId} - product not found skipping.");
                    continue;
                }
                var shipmentDateType = s.ShipmentDateType == ARTMISConsts.OrderDeliveryDateTypes.ActualDeliveryDate ? ShipmentDateType.AcDD :
                    s.ShipmentDateType == ARTMISConsts.OrderDeliveryDateTypes.EstimatedDeliveryDate ? ShipmentDateType.EDD :
                    ShipmentDateType.RDD;
                var totalQuantity = s.OrderedQuantity * shipmentItem.BaseUnitMultiplier;
                report.AddOrUpdateShipment(s.Id, product.Id, s.ShipmentDate, shipmentDateType, totalQuantity);
            }

            var lastReport = lastPeriodReports.Single(r => r.CountryId == report.CountryId);
            if (lastReport != null && lastReport.ProductShipments != null)
            {
                var nonPmiShipments = lastReport.GetNonPMIShipments();
                foreach (var s in nonPmiShipments)
                {
                    report.AddOrUpdateShipment(Guid.NewGuid(), s.ProgramId, s.ProductId, s.Supplier, s.ShipmentDate, s.ShipmentDateType, s.Quantity, s.DataSource);
                }
            }
        }
        await Repository.InsertManyAsync(reports);
    }
    
}

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}