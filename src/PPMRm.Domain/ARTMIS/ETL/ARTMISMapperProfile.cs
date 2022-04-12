using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMRm.ARTMIS.ETL
{
    public class ARTMISMapperProfile : Profile    
    {
        public ARTMISMapperProfile()
        {
            CreateMap<OrderEto, RemoteOrderEvent>()
                .ForMember(dest => dest.FileTimestamp, opt => opt.MapFrom(src => Convert.ToInt64(src.FileNameTimestamp)))
                .ForMember(dest => dest.FileDate, opt => opt.MapFrom(src => Convert.ToInt64(src.FileDateTimeOffset.Date)))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ItemId.Substring(0, 12)));
        }
    }
}
