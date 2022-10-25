using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PPMRm.Core;
using PPMRm.Reports;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PPMRm.Web.Pages.Reports
{
    public class PeriodModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<string> SelectedCountries { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CountryIds { get; set; }

        [BindProperty]
        public List<SelectListItem> Countries { get; set; }
        IReportAppService ReportAppService { get; }
        ICountryRepository CountryRepository { get; }

        public PeriodModel(IReportAppService reportAppService, ICountryRepository countryRepository)
        {
            ReportAppService = reportAppService;
            CountryRepository = countryRepository;
        }

        public async Task OnGetAsync()
        {
            SelectedCountries = CountryIds?.Split(',').ToList();
            var allCountries = await CountryRepository.GetUserCountriesAsync();
            Countries = allCountries.OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            if (SelectedCountries == null || !SelectedCountries.Any())
            {
                SelectedCountries = allCountries.Select(c => c.Id).ToList();
            }
            PeriodSummary = await ReportAppService.GetAsync(Id, SelectedCountries);
        }

        public int SelectedPeriodId => Id;
        public PeriodSummaryDto PeriodSummary { get; set; }

    }
}
