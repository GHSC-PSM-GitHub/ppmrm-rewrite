using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPMRm.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.Web.Pages.PeriodReports
{
    public class IndexModel : PageModel
    {
        IRepository<Country, string> CountryRepository { get; }
        public IndexModel(IRepository<Country, string> countryRepository)
        {
            CountryRepository = countryRepository;

            var allCountries = CountryRepository.OrderBy(c => c.Name).ToList();
            Countries = allCountries.Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            SelectedCountries = Countries.Select(c => c.Value).ToList();
            Months = Enumerable.Range(1, 12).Select(i => new SelectListItem { Value = $"{i}", Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i) }).ToList();
            SelectedMonth = 12; // TODO: Get latest month from period repo
            Years = Enumerable.Range(2021,2).Select(i => new SelectListItem { Value = $"{i}", Text = $"{i}" }).ToList();
            SelectedYear = 2012; // TODO: Get latest year from period repo
            
        }
        public void OnGet()
        {
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
