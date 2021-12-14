using AutoMapper;
using PPMRm.Core;

namespace PPMRm
{
    public class PPMRmApplicationAutoMapperProfile : Profile
    {
        public PPMRmApplicationAutoMapperProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<Program, ProgramDto>();
        }
    }
}
