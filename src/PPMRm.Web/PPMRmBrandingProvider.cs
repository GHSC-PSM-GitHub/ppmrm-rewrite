using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace PPMRm.Web
{
    [Dependency(ReplaceServices = true)]
    public class PPMRmBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "PPMRm";
    }
}
