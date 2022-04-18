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

namespace PPMRm.Web.Pages.Reports
{
    public class IndexModel : PPMRmPageModel
    {
        ICountryRepository CountryRepository { get; }

        public IndexModel(ICountryRepository countryRepository)
        {
            CountryRepository = countryRepository;
        }
        public async Task OnGetAsync()
        {
            var allCountries = await CountryRepository.GetUserCountriesAsync();
            Countries = allCountries.OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            SelectedCountries = Countries.Select(c => c.Value).ToList();
            Months = Enumerable.Range(1, 12).Select(i => new SelectListItem { Value = $"{i}", Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i) }).ToList();
            SelectedMonth = 3; // TODO: Get latest month from period repo
            Years = Enumerable.Range(2021, 2).Select(i => new SelectListItem { Value = $"{i}", Text = $"{i}" }).ToList();
            SelectedYear = 2022; // TODO: Get latest year from period repo
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
