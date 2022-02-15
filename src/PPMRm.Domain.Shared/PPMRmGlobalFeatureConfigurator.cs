using Volo.Abp.GlobalFeatures;
using Volo.Abp.Threading;

namespace PPMRm
{
    public static class PPMRmGlobalFeatureConfigurator
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
                {
                    cmsKit.Pages.Enable();
                    cmsKit.Menu.Enable();
                });
                
            });
        }
    }
}
