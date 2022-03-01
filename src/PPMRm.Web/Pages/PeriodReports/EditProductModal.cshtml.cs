using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPMRm.PeriodReports;
using PPMRm.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
namespace PPMRm.Web.Pages.PeriodReports
{
    public class EditProductModalModel : PPMRmPageModel
    {
        [DisplayName("Product")]
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        [DisplayName("Program")]
        [BindProperty(SupportsGet = true)]
        public int ProgramId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PeriodReportId { get; set; }

        IPeriodReportAppService AppService { get; }
        IRepository<Product, string> ProductRepository { get; }
        IRepository<Core.Program, int> ProgramRepository { get; }

        public List<SelectListItem> Products { get; set; }

        public List<SelectListItem> Programs { get; set; }
        public List<SelectListItem> SOHLevelOptions { get; set; }
        [DisplayName("SOH Levels")]
        [BindProperty]
        public List<string> SOHLevels { get; set; } = new();
        public CreateUpdateProgramProductViewModel Product { get; set; } = new();

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
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await Task.CompletedTask;

            return NoContent();
        }
    }
}
