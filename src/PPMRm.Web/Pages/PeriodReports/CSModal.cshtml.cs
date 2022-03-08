using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PPMRm.PeriodReports;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace PPMRm.Web.Pages.PeriodReports
{
    public class CSModalModel : PPMRmPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public CSUpdateViewModel CSUpdates { get; set; } = new();

        IPeriodReportAppService AppService { get; }

        public CSModalModel(IPeriodReportAppService appService)
        {
            AppService = appService;
        }
        public async Task OnGetAsync()
        {
            var csUpdatesDto = await AppService.GetCSUpdatesAsync(Id);
            Title = csUpdatesDto.Title;
            CSUpdates = ObjectMapper.Map<CommoditySecurityUpdatesDto, CSUpdateViewModel>(csUpdatesDto);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var csUpdatesDto = ObjectMapper.Map<CSUpdateViewModel, CommoditySecurityUpdatesDto>(CSUpdates);
            await AppService.UpdateCSUpdatesAsync(csUpdatesDto.PeriodReportId, csUpdatesDto);
            return NoContent();
        }
    }

    public class CSUpdateViewModel
    {
        [HiddenInput]
        public string PeriodReportId { get; set; }
        [DisplayName("Logistics Management Information System (LMIS)")]
        [TextArea]
        [StringLength(PeriodReportConsts.DataValidation.CSUpdatesMaxLength)]
        public string LogisticsManagementInformationSystem { get; set; }
        [DisplayName("Governance And Financing")]
        [TextArea]
        [StringLength(PeriodReportConsts.DataValidation.CSUpdatesMaxLength)]
        public string GovernanceAndFinancing { get; set; }
        [DisplayName("Human Resources Capacity Development And Training")]
        [TextArea]
        [StringLength(PeriodReportConsts.DataValidation.CSUpdatesMaxLength)]
        public string HumanResourcesCapacityDevelopmentAndTraining { get; set; }
        [DisplayName("Supply Chain Committee Policy And Donor Coordination")]
        [TextArea]
        [StringLength(PeriodReportConsts.DataValidation.CSUpdatesMaxLength)]
        public string SupplyChainCommitteePolicyAndDonorCoordination { get; set; }
        [DisplayName("Product Stock Levels Information")]
        [TextArea]
        [StringLength(PeriodReportConsts.DataValidation.CSUpdatesMaxLength)]
        public string ProductStockLevelsInformation { get; set; }
        [DisplayName("Forecasting and Supply Planning")]
        [TextArea]
        [StringLength(PeriodReportConsts.DataValidation.CSUpdatesMaxLength)]
        public string ForecastingAndSupplyPlanning { get; set; }
        [DisplayName("Procurement Product Information & Registration")]
        [TextArea]
        [StringLength(PeriodReportConsts.DataValidation.CSUpdatesMaxLength)]
        public string ProcurementProductInformationAndRegistration { get; set; }
        [DisplayName("Warehousing and Distribution")]
        [TextArea]
        [StringLength(PeriodReportConsts.DataValidation.CSUpdatesMaxLength)]
        public string WarehousingAndDistribution { get; set; }
    }
}
