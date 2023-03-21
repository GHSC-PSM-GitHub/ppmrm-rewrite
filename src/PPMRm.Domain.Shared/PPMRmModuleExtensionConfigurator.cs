using System;
using System.ComponentModel.DataAnnotations;
using PPMRm.Identity;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace PPMRm
{
    public static class PPMRmModuleExtensionConfigurator
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                ConfigureExistingProperties();
                ConfigureExtraProperties();
            });
        }

        private static void ConfigureExistingProperties()
        {
            /* You can change max lengths for properties of the
             * entities defined in the modules used by your application.
             *
             * Example: Change user and role name max lengths

               IdentityUserConsts.MaxNameLength = 99;
               IdentityRoleConsts.MaxNameLength = 99;

             * Notice: It is not suggested to change property lengths
             * unless you really need it. Go with the standard values wherever possible.
             *
             * If you are using EF Core, you will need to run the add-migration command after your changes.
             */
        }

        private static void ConfigureExtraProperties()
        {
            /* You can configure extra properties for the
             * entities defined in the modules used by your application.
             *
             * This class can be used to define these extra properties
             * with a high level, easy to use API.
             *
             * Example: Add a new property to the user entity of the identity module

               ObjectExtensionManager.Instance.Modules()
                  .ConfigureIdentity(identity =>
                  {
                      identity.ConfigureUser(user =>
                      {
                          user.AddOrUpdateProperty<string>( //property type: string
                              "SocialSecurityNumber", //property name
                              property =>
                              {
                                  //validation rules
                                  property.Attributes.Add(new RequiredAttribute());
                                  property.Attributes.Add(new StringLengthAttribute(64) {MinimumLength = 4});

                                  //...other configurations for this property
                              }
                          );
                      });
                  });

             * See the documentation for more:
             * https://docs.abp.io/en/abp/latest/Module-Entity-Extensions
             */

            OneTimeRunner.Run(() =>
            {
                ObjectExtensionManager.Instance.Modules()
                    .ConfigureIdentity(identity =>
                    {
                        identity.ConfigureUser(user =>
                        {
                            user.AddOrUpdateProperty<string>( //property type: string
                                IdentityConsts.UserExtensionProperties.CountryId, //property name
                                property =>
                                {
                                    //validation rules
                                    property.DefaultValue = null;
                                    property.UI.Lookup.Url = "/api/app/country?MaxResultCount=100&Sorting=name";
                                    property.UI.Lookup.DisplayPropertyName = "name";
                                    //...other configurations for this property
                                    property.DisplayName = new Volo.Abp.Localization.FixedLocalizableString(IdentityConsts.UserExtensionProperties.CountryIdDisplayName);
                                }
                            );
                        });
                    });

                ObjectExtensionManager.Instance.Modules()
                    .ConfigureIdentity(identity =>
                    {
                        identity.ConfigureUser(user =>
                        {
                            user.AddOrUpdateProperty<UserType>( //property type: string
                                IdentityConsts.UserExtensionProperties.UserType, //property name
                                property =>
                                {
                                    property.Attributes.Add(new RequiredAttribute());
                                    //validation rules
                                    property.DefaultValue = UserType.DataProvider;
                                    //...other configurations for this property
                                    property.DisplayName = new Volo.Abp.Localization.FixedLocalizableString(IdentityConsts.UserExtensionProperties.UserTypeDisplayName);
                                }
                            );
                        });
                    });

                ObjectExtensionManager.Instance.Modules()
                    .ConfigureIdentity(identity =>
                    {
                        identity.ConfigureUser(user =>
                        {
                            user.AddOrUpdateProperty<DateTime>( //property type: datetime
                                IdentityConsts.UserExtensionProperties.LastLogin, //property name
                                property =>
                                {
                                    //validation rules
                                    property.DefaultValue = DateTime.Now;
                                    property.UI.OnTable.IsVisible = true;
                                    property.UI.OnCreateForm.IsVisible = false;
                                    property.UI.OnEditForm.IsVisible = false;

                                    property.Api.OnCreate.IsAvailable = true;
                                    property.Api.OnUpdate.IsAvailable = true;
                                    property.Api.OnGet.IsAvailable = true;

                                    property.Attributes.Clear();
                                     
                                    //...other configurations for this property
                                    property.DisplayName = new Volo.Abp.Localization.FixedLocalizableString(IdentityConsts.UserExtensionProperties.LastLoginDisplayName);
                                }
                            );
                        });
                    });
            });
        }
    }
}
