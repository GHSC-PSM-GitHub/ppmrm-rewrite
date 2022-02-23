using AutoMapper;
using PPMRm.Core;
using PPMRm.Items;
using PPMRm.PeriodReports;

namespace PPMRm
{
    public class PPMRmApplicationAutoMapperProfile : Profile
    {
        public PPMRmApplicationAutoMapperProfile()
        {
            CreateMap<Country, CountryDto>();
            CreateMap<Program, ProgramDto>();
            CreateMap<Period, PeriodDto>();
            CreateMap<Item, ItemDto>();
            CreateMap<ARTMIS.Orders.Order, Orders.OrderDto>();
            CreateMap<ARTMIS.Orders.OrderLine, Orders.OrderLineDto>();
            CreateMap<PeriodReport, PeriodReportDto>();
            CreateMap<PeriodReport, PeriodReportDetailDto>();
            CreateMap<CommoditySecurityUpdates, CommoditySecurityUpdatesDto>().ReverseMap();
        }
    }
}
