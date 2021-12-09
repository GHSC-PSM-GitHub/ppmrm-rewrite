using System;
using System.Collections.Generic;
using System.Text;
using PPMRm.Localization;
using Volo.Abp.Application.Services;

namespace PPMRm
{
    /* Inherit your application services from this class.
     */
    public abstract class PPMRmAppService : ApplicationService
    {
        protected PPMRmAppService()
        {
            LocalizationResource = typeof(PPMRmResource);
        }
    }
}
