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
using PPMRm.Products;
using System;
using System.ComponentModel.DataAnnotations;
using PPMRm.Reports;

namespace PPMRm.Web.Pages.Reports
{
    [BindProperties]
    [Authorize]
    public class BuildModel : PPMRmPageModel
    {
        ICountryRepository CountryRepository { get; }
        IRepository<Product, string> ProductRepository { get; }
        IRepository<Core.Program, int> ProgramRepository { get; }
        IReportAppService ReportAppService { get; }

        public BuildModel(ICountryRepository countryRepository, IRepository<Product, string> productRepository, IRepository<Core.Program, int> programRepository, IReportAppService reportAppService)
        {
            CountryRepository = countryRepository;
            ProductRepository = productRepository;
            ProgramRepository = programRepository;
            ReportAppService = reportAppService;
        }
        public async Task OnGetAsync()
        {
            var allCountries = await CountryRepository.GetUserCountriesAsync();
            Countries = allCountries.OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            SelectedCountries = Countries.Select(c => c.Value).ToList();
            var allProducts = await ProductRepository.GetQueryableAsync();
            Products = allProducts.OrderBy(p => p.Name).ToList().Select(p => new SelectListItem { Value = p.Id, Text = p.Name }).ToList();
            SelectedProducts = Products.Select(p => p.Value).ToList();
            var allPrograms = await ProgramRepository.GetQueryableAsync();
            Programs = allPrograms.OrderBy(p => p.Name).ToList().Select(p => new SelectListItem { Value = $"{p.Id}", Text = p.Name }).ToList();
            SelectedPrograms = Programs.Select(p => p.Value).ToList();
            Months = Enumerable.Range(1, 12).Select(i => new SelectListItem { Value = $"{i}", Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i) }).ToList();
            SelectedMonth = 3; // TODO: Get latest month from period repo
            Years = Enumerable.Range(2021, 2).Select(i => new SelectListItem { Value = $"{i}", Text = $"{i}" }).ToList();
            SelectedYear = 2022; // TODO: Get latest year from period repo
        }

        public async Task OnPostAsync()
        {
            var allCountries = await CountryRepository.GetUserCountriesAsync();
            Countries = allCountries.OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            //SelectedCountries = Countries.Select(c => c.Value).ToList();
            var allProducts = await ProductRepository.GetQueryableAsync();
            Products = allProducts.OrderBy(p => p.Name).ToList().Select(p => new SelectListItem { Value = p.Id, Text = p.Name }).ToList();
            //SelectedProducts = Products.Select(p => p.Value).ToList();
            var allPrograms = await ProgramRepository.GetQueryableAsync();
            Programs = allPrograms.OrderBy(p => p.Name).ToList().Select(p => new SelectListItem { Value = $"{p.Id}", Text = p.Name }).ToList();
            //var allPrograms = await ProgramRepository.GetQueryableAsync();
            var request = new GetStockStatusDto
            {
                CountryIds = SelectedCountries ?? new List<string>(),
                ProductIds = SelectedProducts ?? new List<string>(),
                ProgramIds = SelectedPrograms?.Select(x => Convert.ToInt32(x)).ToList() ?? new List<int>(),
                StartPeriodId = StartPeriodId,
                EndPeriodId = EndPeriodId
            };

            Results = await ReportAppService.GetStockStatusAsync(request);
        }
        public List<SelectListItem> Countries { get; set; } = new();
        public List<SelectListItem> Products { get; set; } = new();
        public List<SelectListItem> Programs { get; set; } = new();
        public List<SelectListItem> Years { get; set; } = new();
        public List<SelectListItem> Months { get; set; } = new();
        [DisplayName("Countries")]
        [Required]
        public List<string> SelectedCountries { get; set; } = new();
        [Required]
        [DisplayName("Products")]
        public List<string> SelectedProducts { get; set; } = new();
        [Required]
        [DisplayName("Programs")]
        public List<string> SelectedPrograms { get; set; }
        [DisplayName("Year")]
        public int SelectedYear { get; set; }
        [DisplayName("Period")]
        public int SelectedMonth { get; set; }
        [Required]
        public DateTime? StartPeriod { get; set; }
        [Required]
        public DateTime? EndPeriod { get; set; }

        public List<StockStatusDto> Results { get; set; }

        public int ShipmentColumns => Results != null && Results.Any() ? Results.Max(x => x.Shipments?.Count ?? 0) : 0;

        public int StartPeriodId => Convert.ToInt32(StartPeriod.Value.ToString("yyyyMM"));
        public int EndPeriodId => Convert.ToInt32(EndPeriod.Value.ToString("yyyyMM"));
    }
}
