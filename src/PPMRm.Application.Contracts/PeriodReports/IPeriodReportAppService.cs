using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PPMRm.PeriodReports
{
    public interface IPeriodReportAppService : IReadOnlyAppService<PeriodReportDetailDto, PeriodReportDto, string, GetPeriodReportDto>
    {
        //Task<PeriodReportDetailDto> GetAsync(string countryId, int period);
        Task<CommoditySecurityUpdatesDto> GetCSUpdatesAsync(string id);
        Task UpdateCSUpdatesAsync(string id, CommoditySecurityUpdatesDto csUpdates);

        Task<List<ProgramProductDto>> GetDetailsAsync(string id, int programId);

        Task<ProgramProductDto> GetProgramProductAsync(string id, int programId, string productId);
        Task AddOrUpdateProgramProductAsync(string id, int programId, string productId, CreateUpdateProgramProductDto productInfo);

        Task DeleteShipmentAsync(string id, Guid shipmentId);
        Task AddShipmentAsync(string id, int programId, string productId, CreateUpdateShipmentDto shipment);
        Task<CreateUpdateShipmentDto> GetShipmentAsync(string id, Guid shipmentId);

        Task UpdateShipmentAsync(string id, Guid shipmentId, CreateUpdateShipmentDto shipment);

        Task OpenAsync(string id);
        Task MarkAsFinalAsync(string id);
        Task ReopenAsync(string id);
        Task CloseAsync(string id);

        Task DeleteProgramProductAsync(string id, int programId, string productId);
    }
}
