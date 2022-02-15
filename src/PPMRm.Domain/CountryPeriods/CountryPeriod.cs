using System;
using PPMRm.Core;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Values;
using System.Collections.Generic;

namespace PPMRm.CountryPeriods
{
    public class CountryPeriod : AuditedAggregateRoot<Guid>
    {
        protected CountryPeriod()
        {

        }
        internal CountryPeriod(string countryId, int periodId)
        {
            CountryId = countryId;
            PeriodId = periodId;
            CommoditySecurityUpdates = new CommoditySecurityUpdates();
            ProductShipments = new List<ProductShipment>();
            ProductStocks = new List<ProductStock>();
        }

        #region Properties
        public string CountryId { get; private set; }
        public int PeriodId { get; private set; }
        public CountryPeriodStatus Status { get; private set; }
        public CommoditySecurityUpdates CommoditySecurityUpdates { get; private set; }
        public virtual ICollection<ProductStock> ProductStocks { get; private set; }
        public virtual ICollection<ProductShipment> ProductShipments { get; private set; }
        #endregion

        #region Methods
        public void Open() => throw new NotImplementedException();
        public void Lock() => throw new NotImplementedException();
        public void Close() => throw new NotImplementedException();
        public void AddShipment()
        {

        }
        #endregion

    }

    public class CommoditySecurityUpdates : ValueObject
    {
        public string ForecastingAndSupplyPlanning { get; set; }
        public string ProcurementProductInformationAndRegistration { get; set; }
        public string WarehousingAndDistribution { get; set; }
        public string LogisticsManagementInformationSystem { get; set; }
        public string GovernanceAndFinancing { get; set; }
        public string HumanResourcesCapacityDevelopmentAndTraining { get; set; }
        public string SupplyChainCommitteePolicyAndDonorCoordination { get; set; }
        public string ProductStockLevelsInformation { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ForecastingAndSupplyPlanning;
            yield return ProcurementProductInformationAndRegistration;
            yield return WarehousingAndDistribution;
            yield return LogisticsManagementInformationSystem;
            yield return GovernanceAndFinancing;
            yield return HumanResourcesCapacityDevelopmentAndTraining;
            yield return SupplyChainCommitteePolicyAndDonorCoordination;
            yield return ProductStockLevelsInformation;
        }
    }

    public class ProductStock : AuditedEntity
    {
        public Guid CountryPeriodId { get; set; }
        public Programs ProgramId { get; set; }
        public string ProductId { get; set; }
        public decimal? StockOnHand { get; set; }
        public DateTime DateOfStockOnHand { get; set; }
        public decimal AverageMonthlyConsumption { get; set; }
        public SourceOfConsumption SourceOfConsumption { get; set; }
        public string ActionRecommended { get; set; }
        public DateTime? DateActionRecommendedBy { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { CountryPeriodId, ProgramId, ProductId };
        }
    }

    public class ProductShipment : AuditedEntity<Guid>
    {
        public Guid CountryProgramId { get; set; }
        public Programs ProgramId { get; set; }
        public string ProductId { get; set; }

    }
}
