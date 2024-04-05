using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PPMRm.Localization;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.CmsKit;

namespace PPMRm
{
    [DependsOn(
        typeof(PPMRmApplicationContractsModule),
        typeof(AbpAccountHttpApiModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpTenantManagementHttpApiModule),
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(AbpSettingManagementHttpApiModule)
        )]
    [DependsOn(typeof(CmsKitHttpApiModule))]
    public class PPMRmHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureLocalization();

            ConfigureIdentityOptions(context);
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<PPMRmResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }
        
        
        private void ConfigureIdentityOptions(ServiceConfigurationContext context)
        {
            context.Services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });
        }
    }
}
