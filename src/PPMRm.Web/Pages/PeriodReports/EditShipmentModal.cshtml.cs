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
    public class EditShipmentModalModel : PPMRmPageModel
    {
        [DisplayName("Product")]
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        [BindProperty(SupportsGet = true)]
        [DisplayName("Program")]
        public int ProgramId { get; set; }
        [BindProperty(SupportsGet = true)]
        [HiddenInput]
        public string PeriodReportId { get; set; }

        [BindProperty]
        public AddShipmentViewModel Shipment { get; set; } = new();
        public List<SelectListItem> Products { get; set; }

        public List<SelectListItem> Programs { get; set; }
        IPeriodReportAppService AppService { get; }
        IRepository<Product, string> ProductRepository { get; }
        IRepository<Core.Program, int> ProgramRepository { get; }

        public List<SelectListItem> Suppliers { get; set; }
        public int Supplier { get; set; }

        public EditShipmentModalModel(IPeriodReportAppService appService, IRepository<Product, string> productRepository, IRepository<Core.Program, int> programRepository)
        {
            AppService = appService;
            ProductRepository = productRepository;
            ProgramRepository = programRepository;
        }
        public async Task OnGetAsync(string periodReportId, int programId, string id, Guid shipmentId)
        {
            Suppliers = Enum.GetValues<Supplier>().Select(s => new SelectListItem { Value = $"{(int)s}", Text = s.ToString() }).ToList();
            Products = (await ProductRepository.ToListAsync()).Where(p => id == null || p.Id == id).OrderBy(p => p.Name).Select(p => new SelectListItem { Value = p.Id, Text = p.Name }).ToList();
            Programs = (await ProgramRepository.ToListAsync()).Select(p => new SelectListItem { Value = $"{p.Id}", Text = p.Name }).ToList();
            var existing = await AppService.GetShipmentAsync(periodReportId, shipmentId);
            
            Shipment = new AddShipmentViewModel
            {
                ProgramId = programId,
                PeriodReportId = periodReportId,
                Productid = id,
                ShipmentId = shipmentId,
                DataSource = existing.DataSource ?? ShipmentDataSource.CountryTeam,
                Quantity= (int)existing.Quantity,
                ShipmentDate = existing.ShipmentDate,
                ShipmentDateType = existing.ShipmentDateType,
                Supplier = existing.Supplier
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var shipmentDto = new CreateUpdateShipmentDto
            {
                Id = Shipment.ShipmentId,
                Supplier = Shipment.Supplier,
                ShipmentDate = Shipment.ShipmentDate,
                ShipmentDateType = Shipment.ShipmentDateType,
                Quantity = Shipment.Quantity,
                DataSource = Shipment.DataSource
            };
            await AppService.UpdateShipmentAsync(Shipment.PeriodReportId, Shipment.ShipmentId.Value, shipmentDto);
            return NoContent();
        }
    }
}
