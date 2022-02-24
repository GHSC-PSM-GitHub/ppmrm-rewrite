using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PPMRm.PeriodReports;
using System.Threading.Tasks;

namespace PPMRm.Web.Pages.PeriodReports
{
    public class EditModel : PageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public PeriodReportDetailDto PeriodReport { get; set; }
        IPeriodReportAppService AppService { get; }
        public EditModel(IPeriodReportAppService periodReportAppService)
        {
            AppService = periodReportAppService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            PeriodReport = await AppService.GetAsync(Id);
            if (PeriodReport == null) return NotFound();
            return Page();
        }
    }
}
