using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPMRm.Core;
using PPMRm.Orders;

namespace PPMRm.Web.Pages.Orders
{
    public class IndexModel : PageModel
    {
        ICountryAppService CountryAppService { get; }
        IPeriodAppService PeriodAppService { get; }
        IOrderAppService OrderAppService { get; }
        public IndexModel(ICountryAppService countryAppService, IPeriodAppService periodAppService, IOrderAppService orderAppService, IDocumentSession session)
        {
            PeriodAppService = periodAppService;
            CountryAppService = countryAppService;
            OrderAppService = orderAppService;
            var filters = (OrderAppService.GetFiltersAsync()).Result;
            Countries = filters.Item1.Select(c => new SelectListItem { Value = c.ARTMISName, Text = c.Name }).ToList();
            Periods = filters.Item2.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ShortName }).ToList();
            SelectedCountries = Countries.Select(c => c.Value).ToList();
            Products = session.Query<PPMRm.Items.Item>().Select(i => new SelectListItem { Value = i.Id, Text = i.Name }).ToList();
            SelectedProducts = Products.Select(p => p.Value).ToList();
        }
        public List<SelectListItem> Countries { get; set; } = new();
        public List<SelectListItem> Periods { get; set; } = new();
        public List<SelectListItem> Products { get; set; } = new();
        [DisplayName("Countries")]
        public List<string> SelectedCountries { get; set; } = new();
        [DisplayName("Products")]
        public List<string> SelectedProducts { get; set; } = new();
        public string SelectedPeriod { get; set; }
        public void OnGet()
        {
            
            //Periods = (await PeriodAppService.GetListAsync(new GetPeriodListDto
            //{
            //    StartMonth = 12,
            //    StartYear = 2021,
            //    EndMonth = 12,
            //    EndYear = 2022

            //})).Items.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ShortName }).ToList();
        }
    }
}
