using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PPMRm.Web.Pages.CountryPeriods.ViewModels;

namespace PPMRm.Web.Pages.CountryPeriods
{
    public class CSUpdatesModel : PageModel
    {
        [BindProperty]
        public CSUpdateViewModel CSUpdates { get; set; }

        public void OnGet()
        {
            CSUpdates = new CSUpdateViewModel();
        }
    }
}
