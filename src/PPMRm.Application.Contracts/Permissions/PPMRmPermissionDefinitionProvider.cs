using PPMRm.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace PPMRm.Permissions
{
    public class PPMRmPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(PPMRmPermissions.GroupName);
            //Define your own permissions here. Example:
            //myGroup.AddPermission(PPMRmPermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PPMRmResource>(name);
        }
    }
}
