using AutoMapper;
using PPMRm.Core;
using PPMRm.PeriodReports;
using PPMRm.Web.Pages.Countries;
using PPMRm.Web.Pages.PeriodReports;

namespace PPMRm.Web
{
    public class PPMRmWebAutoMapperProfile : Profile
    {
        public PPMRmWebAutoMapperProfile()
        {
            CreateMap<CommoditySecurityUpdatesDto, CSUpdateViewModel>().ReverseMap();
            CreateMap<CountryDto, EditCountryViewModel>();
            CreateMap<UpdateCountryDto, EditCountryViewModel>().ReverseMap();
            //Define your AutoMapper configuration here for the Web project.
        }
    }
}
