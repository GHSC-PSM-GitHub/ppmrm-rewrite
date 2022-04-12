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

        [BindProperty]
        public PeriodReportDetailDto PeriodReport { get; set; }

        [BindProperty]
        public bool IsReadonly => PeriodReport.ReportStatus != PeriodReportStatus.Open && PeriodReport.ReportStatus != PeriodReportStatus.Reopened;
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
