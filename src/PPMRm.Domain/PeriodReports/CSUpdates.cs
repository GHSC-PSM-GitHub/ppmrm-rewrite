using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Values;

namespace PPMRm.PeriodReports
{
    public class CommoditySecurityUpdates : Entity<string>
    {
        public string ForecastingAndSupplyPlanning { get; set; }
        public string ProcurementProductInformationAndRegistration { get; set; }
        public string WarehousingAndDistribution { get; set; }
        public string LogisticsManagementInformationSystem { get; set; }
        public string GovernanceAndFinancing { get; set; }
        public string HumanResourcesCapacityDevelopmentAndTraining { get; set; }
        public string SupplyChainCommitteePolicyAndDonorCoordination { get; set; }
        public string ProductStockLevelsInformation { get; set; }
    }
}
