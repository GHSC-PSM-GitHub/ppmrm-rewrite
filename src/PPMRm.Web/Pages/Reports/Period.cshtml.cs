using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PPMRm.Reports;
using System.Threading.Tasks;

namespace PPMRm.Web.Pages.Reports
{
    public class PeriodModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        IReportAppService ReportAppService { get; }

        public PeriodModel(IReportAppService reportAppService)
        {
            ReportAppService = reportAppService;
        }

        public async Task OnGetAsync()
        {
            var summary = await ReportAppService.GetAsync(Id);
            PeriodSummary = summary;
        }

        public int SelectedPeriodId => Id;
        public PeriodSummaryDto PeriodSummary { get; set; }

    }
}
