//using AutoMapper;
//using Monitoring.Core.Dtos;
//using Monitoring.Db.Models;

//namespace Monitoring.Api.Automapper.Profiles
//{
//    public class PulsProfile : Profile
//    {
//        public PulsProfile()
//        {
//            CreateMap<Puls, PulsDto>().ReverseMap()
//                .ForMember(dest => dest.PulseType, opt => opt.MapFrom(x => x.PulseType))
//                .ForMember(dest => dest.Nature, opt => opt.MapFrom(x => x.Nature))
//                .ForMember(dest => dest.BoardId, opt => opt.Ignore())
//                .ForMember(dest => dest.Board, opt => opt.Ignore());


//            CreateMap<List<Puls>, List<PulsDto>>().ReverseMap();
//        }
//    }
//}
