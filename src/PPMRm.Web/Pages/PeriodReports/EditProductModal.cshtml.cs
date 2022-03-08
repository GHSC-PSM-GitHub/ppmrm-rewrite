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
    public class EditProductModalModel : PPMRmPageModel
    {
        [BindProperty]
        public string ProductName { get; set; }
        [BindProperty]
        public string ProgramName { get; set; }

        IPeriodReportAppService AppService { get; }
        IRepository<Product, string> ProductRepository { get; }
        IRepository<Core.Program, int> ProgramRepository { get; }

        public List<SelectListItem> Products { get; set; }

        public List<SelectListItem> Programs { get; set; }
        public List<SelectListItem> SOHLevelOptions { get; set; }
        [BindProperty]
        
        public UpdateProgramProductViewModel Product { get; set; } = new();

        public EditProductModalModel(IPeriodReportAppService appService, IRepository<Product, string> productRepository, IRepository<Core.Program, int> programRepository)
        {
            AppService = appService;
            ProductRepository = productRepository;
            ProgramRepository = programRepository;
        }
        public async Task OnGetAsync(string id, string periodReportId, int programId)
        {
            Products = (await ProductRepository.ToListAsync()).Select(p => new SelectListItem { Value = p.Id, Text = p.Name}).ToList();
            Programs = (await ProgramRepository.ToListAsync()).Select(p => new SelectListItem { Value = $"{p.Id}", Text = p.Name }).ToList();
            SOHLevelOptions = Enum.GetValues<SOHLevel>().Select(l => new SelectListItem { Value = $"{(int)l}", Text = L[$"Enum:SOHLevel:{(int)l}"] }).ToList();
            ProductName = Products.SingleOrDefault(p => p.Value == $"{id}")?.Text;
            ProgramName = Programs.SingleOrDefault(p => p.Value == $"{programId}")?.Text;
            var programProduct = await AppService.GetProgramProductAsync(periodReportId, programId, id);
            Product = new UpdateProgramProductViewModel
            {
                ProductId = id,
                ProgramId = programId,
                PeriodReportId = periodReportId,
                SOH = (int?)programProduct?.SOH.GetValueOrDefault() ?? default,
                AMC = (int?)programProduct?.AMC.GetValueOrDefault() ?? default,
                ActionRecommended = programProduct?.ActionRecommended,
                DateActionNeededBy = programProduct?.DateActionNeededBy,
                DateOfSOH = programProduct?.DateOfSOH,
                SOHLevels = new List<string>(),
                SourceOfConsumption = SourceOfConsumption.Forecasted // TODO: Fix hard-code
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var programProductDto = new CreateUpdateProgramProductDto
            {
                SOHLevels = Product.SOHLevels.Select(l => Enum.Parse<SOHLevel>(l)).Distinct().ToList(),
                SOH = Product.SOH,
                DateOfSOH = Product.DateOfSOH,
                AMC = Product.AMC,
                SourceOfConsumption = Product.SourceOfConsumption,
                ActionRecommended = Product.ActionRecommended,
                DateActionNeededBy = Product.DateActionNeededBy
            };
            await AppService.AddOrUpdateProgramProductAsync(Product.PeriodReportId, Product.ProgramId, Product.ProductId, programProductDto);
            return NoContent();
        }
    }

    public class UpdateProgramProductViewModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public int ProgramId { get; set; }
        [BindProperty(SupportsGet = true)]
        [HiddenInput]
        public string ProductId { get; set; }
        [BindProperty(SupportsGet = true)]
        [HiddenInput]
        public string PeriodReportId { get; set; }
        [SelectItems("SOHLevelOptions")]
        [DisplayName("SOH Levels")]
        [BindProperty]
        public List<string> SOHLevels { get; set; } = new();
        [DisplayName("Stock on Hand (SOH)")]
        [Required]
        public int SOH { get; set; }
        [DisplayName("Date of SOH")]
        [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfSOH { get; set; }
        [DisplayName("Average Monthly Consumption")]
        public int AMC { get; set; }
        [DisplayName("MOS")]
        [DisabledInput]
        public decimal MOS { get; set; }
        [DisplayName("Source Of Consumption")]
        public SourceOfConsumption SourceOfConsumption { get; set; }
        [DisplayName("Action Recommended")]
        [TextArea()]
        [StringLength(PeriodReportConsts.DataValidation.ActionRecommendedMaxLength)]
        public string ActionRecommended { get; set; }
        [DisplayName("Date Action Needed By")]
        [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateActionNeededBy { get; set; }
    }


}
