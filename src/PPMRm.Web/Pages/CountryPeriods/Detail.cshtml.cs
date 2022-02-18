using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PPMRm.Web.Pages.CountryPeriods.ViewModels;

namespace PPMRm.Web.Pages.CountryPeriods
{
    public class DetailModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        public CountryPeriodDetailViewModel CountryPeriod { get; set; }

        //ICountryPeriodAppService CountryPeriodAppService { get; }

        //public DetailModel(ICountryPeriodAppService countryPeriodAppService)
        //{
        //    CountryPeriodAppService = countryPeriodAppService;
        //}

        public void OnGet()
        {
            CountryPeriod = new CountryPeriodDetailViewModel() { CommoditySecurityUpdates = new CSUpdateViewModel() };

        }
    }
}
