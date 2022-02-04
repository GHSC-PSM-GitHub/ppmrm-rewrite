using AutoMapper;
using PPMRm.Core;
using PPMRm.Items;

namespace PPMRm
{
    public class PPMRmApplicationAutoMapperProfile : Profile
    {
        public PPMRmApplicationAutoMapperProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<Program, ProgramDto>();
            CreateMap<Period, PeriodDto>();
            CreateMap<Item, ItemDto>();
        }
    }
}
