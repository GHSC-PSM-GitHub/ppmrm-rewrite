using System;
using PPMRm.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace PPMRm.Permissions
{
    public class PPMRmPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public PPMRmPermissionDefinitionProvider()
        {
        }

        public override void Define(IPermissionDefinitionContext context)
        {
            var ppmrmGroup = context.AddGroup(
                "Core",
                LocalizableString.Create<PPMRmResource>($"Permissions:{PPMRmConsts.Permissions.Core}")
            );

            ppmrmGroup.AddPermission(
                PPMRmConsts.Permissions.DataReviewer,
                LocalizableString.Create<PPMRmResource>($"Permissions:{PPMRmConsts.Permissions.DataReviewer}")
            );

            ppmrmGroup.AddPermission(
                PPMRmConsts.Permissions.DataProvider,
                LocalizableString.Create<PPMRmResource>($"Permissions:{PPMRmConsts.Permissions.DataProvider}")
            );

            ppmrmGroup.AddPermission(
                PPMRmConsts.Permissions.DataAdministrator,
                LocalizableString.Create<PPMRmResource>($"Permissions:{PPMRmConsts.Permissions.DataAdministrator}")
            );


        }
    }
}
