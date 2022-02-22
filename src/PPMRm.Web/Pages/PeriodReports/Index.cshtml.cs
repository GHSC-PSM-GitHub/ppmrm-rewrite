using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPMRm.Core;
using System.Collections.Generic;
using System.ComponentModel;
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
        }
        public async void OnGet()
        {
        }

        public List<SelectListItem> Countries { get; set; } = new();
        public List<SelectListItem> Periods { get; set; } = new();
        [DisplayName("Countries")]
        public List<string> SelectedCountries { get; set; } = new();
    }
}
