using PPMRm.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace PPMRm.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class PPMRmPageModel : AbpPageModel
    {
        protected PPMRmPageModel()
        {
            LocalizationResourceType = typeof(PPMRmResource);
        }
    }
}