using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPMRm.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using PPMRm.Reports;
using System;
using System.ComponentModel.DataAnnotations;

namespace PPMRm.Web.Pages.Reports
{

    [BindProperties]
    public class IndexModel : PPMRmPageModel
    {
        ICountryRepository CountryRepository { get; }
        IReportAppService ReportAppService { get; }

        public IndexModel(ICountryRepository countryRepository, IReportAppService reportAppService)
        {
            CountryRepository = countryRepository;
            ReportAppService = reportAppService;
        }
        public async Task OnGetAsync()
        {
            var allCountries = await CountryRepository.GetUserCountriesAsync();
            Countries = allCountries.OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            SelectedCountries = Countries.Select(c => c.Value).ToList();
            //PeriodSummary = await ReportAppService.GetAsync(202203);
        }

        public async Task OnPostAsync()
        {
            var allCountries = await CountryRepository.GetUserCountriesAsync();
            Countries = allCountries.OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            if (ModelState.IsValid)
            {
                SelectedPeriodId = Convert.ToInt32(SelectedPeriod.Value.ToString("yyyyMM"));
                PeriodSummary = await ReportAppService.GetAsync(SelectedPeriodId.Value);
            }
            
        }

        public PeriodSummaryDto PeriodSummary { get; set; }        
        public List<SelectListItem> Countries { get; set; } = new();
        public List<SelectListItem> Years { get; set; } = new();
        public List<SelectListItem> Months { get; set; } = new();
        [DisplayName("Countries")]
        public List<string> SelectedCountries { get; set; } = new();
        [Required]
        public DateTime? SelectedPeriod { get; set; }
        [HiddenInput]
        public int? SelectedPeriodId { get; set; }

    }
}
