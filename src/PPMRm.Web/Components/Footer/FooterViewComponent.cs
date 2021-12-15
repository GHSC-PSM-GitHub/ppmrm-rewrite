using System;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace PPMRm.Web.Components.Footer
{
    public class FooterViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("/Components/Footer/Default.cshtml");
        }
    }
}
