using PPMRm.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace PPMRm.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class PPMRmController : AbpController
    {
        protected PPMRmController()
        {
            LocalizationResource = typeof(PPMRmResource);
        }
    }
}