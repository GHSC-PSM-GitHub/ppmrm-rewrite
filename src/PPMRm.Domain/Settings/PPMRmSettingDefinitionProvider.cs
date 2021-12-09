using Volo.Abp.Settings;

namespace PPMRm.Settings
{
    public class PPMRmSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(PPMRmSettings.MySetting1));
        }
    }
}
