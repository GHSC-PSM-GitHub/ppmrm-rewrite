using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPMRm.PeriodReports;
using PPMRm.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.Web.Pages.PeriodReports
{
    public class EditProgramProductModel : PPMRmPageModel
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
        [BindProperty]
        public bool IsReadonly { get; set; }

        public EditProgramProductModel(IPeriodReportAppService appService, IRepository<Product, string> productRepository, IRepository<Core.Program, int> programRepository)
        {
            AppService = appService;
            ProductRepository = productRepository;
            ProgramRepository = programRepository;
        }
        public async Task OnGetAsync(string id, string periodReportId, int programId)
        {
            Products = (await ProductRepository.ToListAsync()).Select(p => new SelectListItem { Value = p.Id, Text = p.Name }).ToList();
            Programs = (await ProgramRepository.ToListAsync()).Select(p => new SelectListItem { Value = $"{p.Id}", Text = p.Name }).ToList();
            SOHLevelOptions = Enum.GetValues<SOHLevel>().Select(l => new SelectListItem { Value = $"{(int)l}", Text = L[$"Enum:SOHLevel:{l}"] }).ToList();
            ProductName = Products.SingleOrDefault(p => p.Value == $"{id}")?.Text;
            ProgramName = Programs.SingleOrDefault(p => p.Value == $"{programId}")?.Text;
            var programProduct = await AppService.GetProgramProductAsync(periodReportId, programId, id);
            IsReadonly = programProduct.ReportStatus != PeriodReportStatus.Open && programProduct.ReportStatus != PeriodReportStatus.Reopened;
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
                SOHLevels = programProduct?.SOHLevels,
                SourceOfConsumption = programProduct?.SourceOfConsumption ?? SourceOfConsumption.Forecasted,
                OtherSourceOfConsumption = programProduct?.OtherSourceOfConsumption,
                Shipments = programProduct.Shipments
            };
        }
    }
}
