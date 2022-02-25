using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PPMRm.Web.Pages.PeriodReports
{
    public class EditProductModalModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ProgramId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PeriodReportId { get; set; }

        public CreateUpdateProgramProductViewModel Product { get; set; } = new();
        public void OnGet(string id, string periodReportId, int programId)
        {
            Product = new();
        }
    }
}
