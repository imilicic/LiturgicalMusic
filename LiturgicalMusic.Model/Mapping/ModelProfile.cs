using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LiturgicalMusic.DAL;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Model.Mapping
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<ISong, SongEntity>()
                    .ForMember(dest => dest.OtherParts, opt => opt.MapFrom(s => s.Stanzas.Count() > 0));
            CreateMap<SongEntity, ISong>();
            CreateMap<IStanza, StanzaEntity>().ReverseMap();
            CreateMap<IComposer, ComposerEntity>().ReverseMap();
            CreateMap<IInstrumentalPart, InstrumentalPartEntity>().ReverseMap();
            CreateMap<int, LiturgyEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s));
            CreateMap<LiturgyEntity, int>();
        }
    }
}
