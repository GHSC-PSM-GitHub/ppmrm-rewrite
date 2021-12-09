using System.Threading.Tasks;
using PPMRm.Localization;
using PPMRm.MultiTenancy;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace PPMRm.Web.Menus
{
    public class PPMRmMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var administration = context.Menu.GetAdministration();
            var l = context.GetLocalizer<PPMRmResource>();

            context.Menu.Items.Insert(
                0,
                new ApplicationMenuItem(
                    PPMRmMenus.Home,
                    l["Menu:Home"],
                    "~/",
                    icon: "fas fa-home",
                    order: 0
                )
            );

            if(await context.IsGrantedAsync(PPMRmConsts.Permissions.DataProvider))
            {
                context.Menu.GetAdministration().AddItem(
                new ApplicationMenuItem(
                    "DataManagement",
                    l["Menu:DataManagement"],
                    icon: "fa fa-book"
                    ).AddItem(
                        new ApplicationMenuItem(
                            "DataManagement.Products",
                            l["Menu:Products"],
                            url: "/products"
                        )
                    ).AddItem(
                        new ApplicationMenuItem(
                            "DataManagement.Countries",
                            l["Menu:Countries"],
                            url: "/countries"
                        )
                    ).AddItem(
                        new ApplicationMenuItem(
                            "DataManagement.Programs",
                            l["Menu:Programs"],
                            url: "/programs"
                        )
                    )
                );
            }
            

            if (MultiTenancyConsts.IsEnabled)
            {
                administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
            }
            else
            {
                administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
            }

            administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
            administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);
        }
    }
}
