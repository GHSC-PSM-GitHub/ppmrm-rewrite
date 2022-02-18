using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace PPMRm.Web.Pages.PeriodReports
{
    public class CSUpdatesModel : PageModel
    {
        public void OnGet()
        {
            throw new ArgumentException("No open period!");
        }
    }
}
