﻿using PPMRm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace PPMRm.PeriodReports
{
    public class PeriodReport : AggregateRoot<string>
    {
        private PeriodReport()
        {

        }
        internal PeriodReport(string countryId, int periodId) : base($"{countryId}-{periodId}")
        {   
            CountryId = Check.NotNullOrWhiteSpace(countryId, nameof(countryId));
            PeriodId = periodId;
            CommoditySecurityUpdates = new CommoditySecurityUpdates();
            ReportStatus = PeriodReportStatus.Initialized;
            ProductShipments = new List<ProductShipment>();
            ProductStocks = new List<ProductStock>();
        }

        public string CountryId { get; private set; }
        public int PeriodId { get; private set; }
        public CommoditySecurityUpdates CommoditySecurityUpdates { get; private set; }

        public PeriodReportStatus? ReportStatus { get; private set; }

        public ICollection<ProductShipment> ProductShipments { get; private set; }
        public ICollection<ProductStock> ProductStocks { get; private set; }

        public void SetCSUpdates(CommoditySecurityUpdates csUpdates)
        {
            CommoditySecurityUpdates = Check.NotNull(csUpdates, nameof(csUpdates));
        }

        public void AddOrUpdateProgramProduct(int programId, string productId, List<SOHLevel> sohLevels, decimal soh, DateTime? sohDate, decimal amc, SourceOfConsumption sourceOfConsumption, string actionRecommended, DateTime? actionNeededBy, string otherSourceOfConsumption = null)
        {
            if (!ProductStocks.Any(p => p.ProgramId == programId && p.ProductId == productId))
                ProductStocks.Add(new ProductStock(Id, programId, productId));
            var existing = ProductStocks.SingleOrDefault(p => p.ProgramId == programId && p.ProductId == productId);
            existing.Update(sohLevels, soh, sohDate, amc, sourceOfConsumption, actionRecommended, actionNeededBy, otherSourceOfConsumption);
        }

        /// <summary>
        /// TO DO, Move this logic to CountryAggregate
        /// </summary>
        /// <returns></returns>
        public List<int> GetDefaultProgramIds()
        {
            switch (CountryId)
            {
                case CountryConsts.CountryCodes.CongoDRC:
                    return new List<int> { (int)Programs.NationalMalariaProgram, (int)Programs.NationalMalariaProgramGF };
                case CountryConsts.CountryCodes.Uganda:
                    return new List<int> { (int)Programs.PublicSector, (int)Programs.PNFP };
                case CountryConsts.CountryCodes.Myanmar:
                    return new List<int> { (int)Programs.NationalMalariaProgram, (int)Programs.CAPMalaria };
                default:
                    return new List<int> { (int)Programs.NationalMalariaProgram };
            }
        }
        public int GetPMIProgramId() => (int)GetARTMISProgram();

        public List<ProductShipment> GetNonPMIShipments()
        {
            return ProductShipments.Where(s => s.Supplier != Supplier.PMI)?.ToList();
        }
        

        public Programs GetARTMISProgram()
        {
            switch (CountryId)
            {
                case CountryConsts.CountryCodes.Uganda:
                    return Programs.PNFP;
                case CountryConsts.CountryCodes.Myanmar:
                    return Programs.CAPMalaria;
                default:
                    return Programs.NationalMalariaProgram;
            }
        }
        /// <summary>
        /// ARTMIS Shipments
        /// </summary>
        /// <param name="orderLineId"></param>
        /// <param name="productId"></param>
        /// <param name="shipmentDate"></param>
        /// <param name="shipmentDateType"></param>
        /// <param name="quantity"></param>
        public void AddOrUpdateShipment(string orderLineId, string productId, DateTime? shipmentDate, ShipmentDateType shipmentDateType, decimal quantity)
        {
            var shipment = new ProductShipment(Guid.NewGuid(), Id, GetPMIProgramId(), productId, Supplier.PMI, shipmentDate, shipmentDateType, quantity, ShipmentDataSource.ARTMIS);
            ProductShipments.RemoveAll(s => s.Id == shipment.Id);
            ProductShipments.Add(shipment);
        }

        /// <summary>
        /// Add/update other shipments
        /// </summary>
        /// <param name="id"></param>
        /// <param name="programId"></param>
        /// <param name="productId"></param>
        /// <param name="supplier"></param>
        /// <param name="shipmentDate"></param>
        /// <param name="shipmentDateType"></param>
        /// <param name="quantity"></param>
        public async void AddOrUpdateShipment(Guid id, int programId, string productId, Supplier supplier, DateTime? shipmentDate, ShipmentDateType shipmentDateType, decimal quantity, ShipmentDataSource? dataSource = ShipmentDataSource.CountryTeam)
        {
            var nextShipmentDate = shipmentDateType != ShipmentDateType.TBD ? shipmentDate : null;
            var shipment = new ProductShipment(Guid.NewGuid(), Id, programId, productId, supplier, nextShipmentDate, shipmentDateType, quantity, dataSource ?? ShipmentDataSource.CountryTeam);
            ProductShipments.RemoveAll(s => s.Id == id);
            ProductShipments.Add(shipment);
        }

        public void RemoveProgramProduct(int programId, string productId) => ProductStocks.RemoveAll(p => p.ProgramId == programId && p.ProductId == productId);
        public void RemoveShipment(Guid id) => ProductShipments.RemoveAll(s => s.Id == id);

        public void Open() => ReportStatus = PeriodReportStatus.Open;
        public void MarkAsFinal() => ReportStatus = PeriodReportStatus.Final;
        public void Close() => ReportStatus = PeriodReportStatus.Closed;
        public void Reopen() => ReportStatus = PeriodReportStatus.Reopened;

    }
}
