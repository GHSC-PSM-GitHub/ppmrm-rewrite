using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PPMRm.Web.Pages.CountryPeriods
{
    public class ListModel : PageModel
    {
        [BindProperty]
        public string CountryId { get; set; }

        public void OnGet(string countryId)
        {
            CountryId = countryId;
        }
    }
}
