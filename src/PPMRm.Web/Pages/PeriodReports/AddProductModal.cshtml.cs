using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace PPMRm.Web.Pages.PeriodReports
{
    public class AddProductModalModel : PPMRmPageModel
    {
        [BindProperty(SupportsGet =true)]
        public string Id { get; set; }
        [BindProperty(SupportsGet =true)]
        public int ProgramId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PeriodReportId { get; set; }

        public CreateUpdateProgramProductViewModel Product { get; set; } = new();
        public void OnGet(string periodReportId, int programId)
        {
            Product = new();
        }
    }

    public class CreateUpdateProgramProductViewModel
    {
        public decimal SOH { get; set; }
        public DateTime? DateOfSOH { get; set; }
        public decimal AMC { get; set; }
    }
}
