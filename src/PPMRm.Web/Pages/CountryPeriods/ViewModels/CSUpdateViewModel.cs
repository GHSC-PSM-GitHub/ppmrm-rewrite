using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace PPMRm.Web.Pages.CountryPeriods.ViewModels
{
    public class CSUpdateViewModel
    {
        [DisplayName("Forecasting and Supply Planning")]
        [TextArea]
        [StringLength(1000)]
        public string ForecastingAndSupplyPlanning { get; set; }
        [DisplayName("Procurement Product Information And Registration")]
        [TextArea]
        [StringLength(1000)]
        public string ProcurementProductInformationAndRegistration { get; set; }
        [DisplayName("Warehousing And Distribution")]
        [TextArea]
        [StringLength(1000)]
        public string WarehousingAndDistribution { get; set; }
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
