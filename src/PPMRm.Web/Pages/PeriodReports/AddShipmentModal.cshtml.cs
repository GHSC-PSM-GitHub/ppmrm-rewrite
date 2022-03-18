using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPMRm.PeriodReports;
using PPMRm.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.Web.Pages.PeriodReports
{
    public class AddShipmentModalModel : PPMRmPageModel
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
            Shipment = new AddShipmentViewModel
            {
                ProgramId = programId,
                PeriodReportId = periodReportId,
                Productid = id
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var shipmentDto = new CreateUpdateShipmentDto
            {
                Supplier = Shipment.Supplier,
                ShipmentDate = Shipment.ShipmentDate,
                ShipmentDateType = Shipment.ShipmentDateType,
                Quantity = Shipment.Quantity,
                DataSource = Shipment.DataSource
            };
            await AppService.AddShipmentAsync(Shipment.PeriodReportId, Shipment.ProgramId, Shipment.Productid, shipmentDto);
            return NoContent();
        }
    }

    public class AddShipmentViewModel
    {
        [HiddenInput]
        public Guid? ShipmentId { get; set; }
        [HiddenInput]
        public string PeriodReportId { get; set; }
        [HiddenInput]
        public int ProgramId { get; set; }
        [SelectItems("Products")]
        [DisplayName("Product")]
        [BindProperty]
        public string Productid { get; set; }
        public Supplier Supplier { get; set; }
        [DisplayName("Next Shipment Date")]
        [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ShipmentDate { get; set; }
        [DisplayName("Shipment Date Type")]
        public ShipmentDateType ShipmentDateType { get; set; }
        public int Quantity { get; set; }
        [DisplayName("Shipment Date Source")]
        public ShipmentDataSource DataSource { get; set; }

    }
}
