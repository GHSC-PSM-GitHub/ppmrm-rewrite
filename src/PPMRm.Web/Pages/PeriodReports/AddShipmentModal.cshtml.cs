using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace PPMRm.Web.Pages.PeriodReports
{
    public class AddShipmentModalModel : PPMRmPageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ProgramId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PeriodReportId { get; set; }

        public AddShipmentViewModel Shipment { get; set; } = new();
        public void OnGet(string periodReportId, int programId, string id)
        {
            Shipment = new();
        }
    }

    public class AddShipmentViewModel
    {
        public string Supplier { get; set; }
        public DateTime ShipmentDate { get; set; }
    }
}
