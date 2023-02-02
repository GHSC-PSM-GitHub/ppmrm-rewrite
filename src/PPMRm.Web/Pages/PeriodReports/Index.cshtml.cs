using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPMRm.Core;
using PPMRm.PeriodReports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.Web.Pages.PeriodReports
{
    [Authorize]
    public class IndexModel : PageModel
    {
        ICountryRepository CountryRepository { get; }
        IRepository<PeriodReport, string> PeriodReportRepository { get; }
        public IndexModel(ICountryRepository countryRepository, IRepository<PeriodReport, string> periodReportRepository)
        {
            CountryRepository = countryRepository;
            PeriodReportRepository = periodReportRepository;
        }

        public async Task OnGetAsync()
        {
            var allCountries = await CountryRepository.GetUserCountriesAsync();
            var latestReport = PeriodReportRepository.OrderByDescending(x => x.PeriodId).FirstOrDefault();
            var periodId = latestReport?.PeriodId ?? 202205;
            Countries = allCountries.OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            SelectedCountries = Countries.Select(c => c.Value).ToList();
            Months = Enumerable.Range(1, 12).Select(i => new SelectListItem { Value = $"{i}", Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i) }).ToList();
            SelectedMonth = periodId % 100; // TODO: Get latest month from period repo
            Years = Enumerable.Range(2021, 3).Select(i => new SelectListItem { Value = $"{i}", Text = $"{i}" }).ToList();
            SelectedYear = periodId/100; // TODO: Get latest year from period repo
        }

        public List<SelectListItem> Countries { get; set; } = new();
        public List<SelectListItem> Years { get; set; } = new();
        public List<SelectListItem> Months { get; set; } = new();
        [DisplayName("Countries")]
        public List<string> SelectedCountries { get; set; } = new();
        [DisplayName("Year")]
        public int SelectedYear { get; set; }
        [DisplayName("Period")]
        public int SelectedMonth { get; set; }

    }
}
