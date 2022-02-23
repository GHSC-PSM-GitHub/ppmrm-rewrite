using AutoMapper;
using PPMRm.PeriodReports;
using PPMRm.Web.Pages.PeriodReports;

namespace PPMRm.Web
{
    public class PPMRmWebAutoMapperProfile : Profile
    {
        public PPMRmWebAutoMapperProfile()
        {
            CreateMap<CommoditySecurityUpdatesDto, CSUpdateViewModel>().ReverseMap();
            //Define your AutoMapper configuration here for the Web project.
        }
    }
}
