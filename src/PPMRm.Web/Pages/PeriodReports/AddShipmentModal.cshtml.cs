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
    public class AddShipmentModalModel : PPMRmPageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ProgramId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PeriodReportId { get; set; }

        public AddShipmentViewModel Shipment { get; set; } = new();
        public List<SelectListItem> Products { get; set; }

        public List<SelectListItem> Programs { get; set; }
        IPeriodReportAppService AppService { get; }
        IRepository<Product, string> ProductRepository { get; }
        IRepository<Core.Program, int> ProgramRepository { get; }

        public List<SelectListItem> Suppliers { get; set; }
        public int Supplier { get; set; }
        public AddShipmentModalModel(IPeriodReportAppService appService, IRepository<Product, string> productRepository, IRepository<Core.Program, int> programRepository)
        {
            AppService = appService;
            ProductRepository = productRepository;
            ProgramRepository = programRepository;
        }
        public async Task OnGetAsync(string periodReportId, int programId, string id)
        {
            Suppliers = Enum.GetValues<Supplier>().Select(s => new SelectListItem { Value = $"{(int)s}", Text = s.ToString() }).ToList();
            Products = (await ProductRepository.ToListAsync()).OrderBy(p => p.Name).Select(p => new SelectListItem { Value = p.Id, Text = p.Name }).ToList();
            Programs = (await ProgramRepository.ToListAsync()).Select(p => new SelectListItem { Value = $"{p.Id}", Text = p.Name }).ToList();
            Shipment = new();
        }
    }

    public class AddShipmentViewModel
    {
        public Supplier Supplier { get; set; }
        [DisplayName("Next Shipment Date")]
        public DateTime? ShipmentDate { get; set; }
        [DisplayName("Shipment Date Type")]
        public ShipmentDateType ShipmentDateType { get; set; }
        public decimal Quantity { get; set; }

    }
}
