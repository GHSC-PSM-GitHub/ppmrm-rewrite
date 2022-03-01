using System.Threading.Tasks;
using PPMRm.Localization;
using PPMRm.MultiTenancy;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

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
            var currentUser = (ICurrentUser)context.ServiceProvider.GetService(typeof(ICurrentUser));
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

            if (await context.IsGrantedAsync(PPMRmConsts.Permissions.DataReviewer))
            {
                context.Menu.AddItem(
                    new ApplicationMenuItem(
                        "DataEntry",
                       "Data Entry (Period Reports)",
                        icon: "fa fa-order"
                        ).AddItem(
                            new ApplicationMenuItem(
                                "ProductData",
                                "Enter/View Period Reports",
                                url: "/periodreports"
                            )
                        ).AddItem(
                            new ApplicationMenuItem(
                                "DownloadForm",
                                "Download Data Entry Form",
                                url: "/periodreports"
                            )
                        )
                );

                context.Menu.AddItem(
                    new ApplicationMenuItem(
                        "ARTMIS",
                       "ARTMIS",
                        icon: "fa fa-order"
                        ).AddItem(
                            new ApplicationMenuItem(
                                "Items",
                                "Items",
                                url: "/items"
                            )
                        ).AddItem(
                            new ApplicationMenuItem(
                                "Orders",
                                "Orders",
                                url: "/artmis/orderlines"
                            )
                        )
                );

                
            }
            //    context.Menu.GetAdministration().AddItem(
            //    new ApplicationMenuItem(
            //        "DataManagement",
            //        l["Menu:DataManagement"],
            //        icon: "fa fa-book"
            //        ).AddItem(
            //            new ApplicationMenuItem(
            //                "DataManagement.Products",
            //                l["Menu:Products"],
            //                url: "/products"
            //            )
            //        ).AddItem(
            //            new ApplicationMenuItem(
            //                "DataManagement.Countries",
            //                l["Menu:Countries"],
            //                url: "/countries"
            //            )
            //        ).AddItem(
            //            new ApplicationMenuItem(
            //                "DataManagement.Programs",
            //                l["Menu:Programs"],
            //                url: "/programs"
            //            )
            //        )
            //    );

            //context.Menu.AddItem(
            //    new ApplicationMenuItem(
            //        "DataEntry",
            //       "Data Entry & View",
            //        icon: "fa fa-order"
            //        ).AddItem(
            //            new ApplicationMenuItem(
            //                "CSUpdates",
            //                "Enter CS Updates",
            //                url: "/countryperiods/csupdates"
            //            )
            //        ).AddItem(
            //            new ApplicationMenuItem(
            //                "ProductData",
            //                "Enter/View Product Data",
            //                url: "/countries"
            //            )
            //        ).AddItem(
            //            new ApplicationMenuItem(
            //                "DownloadForm",
            //                "Download Data Entry Form",
            //                url: "/shipments"
            //            )
            //        )
            //);

            //context.Menu.AddItem(
            //    new ApplicationMenuItem(
            //        "DataEntry",
            //       "Data Entry & View",
            //        icon: "fa fa-order"
            //        ).AddItem(
            //            new ApplicationMenuItem(
            //                "CSUpdates",
            //                "Enter CS Updates",
            //                url: "/countryperiods/csupdates"
            //            )
            //        ).AddItem(
            //            new ApplicationMenuItem(
            //                "ProductData",
            //                "Enter/View Product Data",
            //                url: "/countries"
            //            )
            //        ).AddItem(
            //            new ApplicationMenuItem(
            //                "DownloadFOrm",
            //                "Download Data Entry Form",
            //                url: "/shipments"
            //            )
            //        )
            //);

            //context.Menu.AddItem(
            //        new ApplicationMenuItem(
            //            "ARTMIS",
            //           "ARTMIS",
            //            icon: "fa fa-order"
            //            ).AddItem(
            //                new ApplicationMenuItem(
            //                    "Items",
            //                    "Items",
            //                    url: "/items"
            //                )
            //            ).AddItem(
            //                new ApplicationMenuItem(
            //                    "Orders",
            //                    "Orders",
            //                    url: "/orders"
            //                )
            //            )
            //    );
            //context.Menu.AddItem(
            //    new ApplicationMenuItem(
            //        "Graphs",
            //       "Graphs",
            //        icon: "fa fa-order"
            //        ).AddItem(
            //            new ApplicationMenuItem(
            //                "Orders",
            //                "Orders",
            //                url: "/orders"
            //            )
            //        ).AddItem(
            //            new ApplicationMenuItem(
            //                "Shipments",
            //                "Shipments",
            //                url: "/shipments"
            //            )
            //        )
            //);
            //context.Menu.AddItem(
            //    new ApplicationMenuItem(
            //        "Reports",
            //       "Indicator Reports",
            //        icon: "fa fa-order"
            //        ).AddItem(
            //            new ApplicationMenuItem(
            //                "Orders",
            //                "Orders",
            //                url: "/orders"
            //            )
            //        ).AddItem(
            //            new ApplicationMenuItem(
            //                "Shipments",
            //                "Shipments",
            //                url: "/shipments"
            //            )
            //        )
            //);
            //}

            if (currentUser.IsAuthenticated)
            {
                //context.Menu.AddItem(new ApplicationMenuItem(
                //    "Help",
                //    l["Menu:Help"],
                //    icon: "fa fa-book",
                //    url: "/pages/help"
                //));
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
