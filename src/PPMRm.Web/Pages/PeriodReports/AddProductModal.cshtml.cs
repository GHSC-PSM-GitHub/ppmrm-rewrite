using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPMRm.PeriodReports;
using PPMRm.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.Web.Pages.PeriodReports
{
    public class AddProductModalModel : PPMRmPageModel
    {
        [DisplayName("Product")]
        [BindProperty(SupportsGet =true)]
        public string Id { get; set; }
        [DisplayName("Program")]
        [BindProperty(SupportsGet =true)]
        public int ProgramId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PeriodReportId { get; set; }
        IPeriodReportAppService AppService { get; }
        IRepository<Product, string> ProductRepository { get; }
        IRepository<Core.Program, int> ProgramRepository { get; }

        public List<SelectListItem> Products { get; set; }

        public List<SelectListItem> Programs { get; set; }

        public List<SelectListItem> SOHLevelOptions { get; set; }
        public List<string> SOHLevels { get; set; } = new();

        public CreateUpdateProgramProductViewModel Product { get; set; } = new();

        public AddProductModalModel(IPeriodReportAppService appService, IRepository<Product, string> productRepository, IRepository<Core.Program, int> programRepository)
        {
            AppService = appService;
            ProductRepository = productRepository;
            ProgramRepository = programRepository;
        }
        public async Task OnGet(string periodReportId, int programId)
        {
            Products = (await ProductRepository.ToListAsync()).OrderBy(p => p.Name).Select(p => new SelectListItem { Value = p.Id, Text = p.Name }).ToList();
            Programs = (await ProgramRepository.ToListAsync()).Select(p => new SelectListItem { Value = $"{p.Id}", Text = p.Name }).ToList();
            SOHLevelOptions = Enum.GetValues<SOHLevel>().Select(l => new SelectListItem { Value = $"{(int)l}", Text = l.ToString() }).ToList();
            Product = new();
        }
    }

    public class CreateUpdateProgramProductViewModel
    {
        [DisplayName("Stock on Hand (SOH)")]
        [Required]
        public decimal SOH { get; set; }
        [DisplayName( "Date of SOH")]
        public DateTime? DateOfSOH { get; set; }
        [DisplayName( "Average Monthly Consumption")]
        public decimal AMC { get; set; }
        [DisplayName( "Source Of Consumption")]
        public SourceOfConsumption SourceOfConsumption { get; set; }
        [DisplayName( "Action Recommended")]
        [TextArea()]
        public string ActionRecommended { get; set; }
        [DisplayName( "Date Action Needed By")]
        public DateTime? DateActionNeededBy { get; set; }
    }

    public enum SourceOfConsumption
    {
        Forecasted,
        LMIS,
        Combined,
        AMD,
        Other
    }
}
