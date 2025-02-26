﻿using AutoMapper;
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
            CreateMap<Country, UpdateCountryDto>();
            CreateMap<Program, ProgramDto>();
            CreateMap<Period, PeriodDto>();
            CreateMap<Item, ItemDto>();
            CreateMap<ARTMIS.Orders.Order, Orders.OrderDto>();
            CreateMap<ARTMIS.Orders.OrderLine, Orders.OrderLineDto>();
            CreateMap<PeriodReport, PeriodReportDto>();
            CreateMap<PeriodReport, PeriodReportDetailDto>();
            CreateMap<ProductShipment, CreateUpdateShipmentDto>();
            CreateMap<CommoditySecurityUpdates, CommoditySecurityUpdatesDto>().ReverseMap();
            CreateMap<ARTMIS.OrderLines.OrderLine, ARTMIS.OrderLines.OrderLineDto>();
        }
    }
}
