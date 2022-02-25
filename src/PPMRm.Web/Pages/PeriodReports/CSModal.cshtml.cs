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
        public async void OnGet(string id)
        {
            var csUpdatesDto = await AppService.GetCSUpdatesAsync(id);
            Title = csUpdatesDto.Name;
            CSUpdates = ObjectMapper.Map<CommoditySecurityUpdatesDto, CSUpdateViewModel>(csUpdatesDto);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var csUpdatesDto = ObjectMapper.Map<CSUpdateViewModel, CommoditySecurityUpdatesDto>(CSUpdates);
            await AppService.UpdateCSUpdatesAsync(Id, csUpdatesDto);
            return NoContent();
        }
    }

    public class CSUpdateViewModel
    {
        [DisplayName("Logistics Management Information System (LMIS)")]
        [TextArea]
        [StringLength(1000)]
        public string LogisticsManagementInformationSystem { get; set; }
        [DisplayName("Governance And Financing")]
        [TextArea]
        [StringLength(1000)]
        public string GovernanceAndFinancing { get; set; }
        [DisplayName("Human Resources Capacity Development And Training")]
        [TextArea]
        [StringLength(1000)]
        public string HumanResourcesCapacityDevelopmentAndTraining { get; set; }
        [DisplayName("Supply Chain Committee Policy And Donor Coordination")]
        [TextArea]
        [StringLength(1000)]
        public string SupplyChainCommitteePolicyAndDonorCoordination { get; set; }
        [DisplayName("Product Stock Levels Information")]
        [TextArea]
        [StringLength(1000)]
        public string ProductStockLevelsInformation { get; set; }
    }
}
