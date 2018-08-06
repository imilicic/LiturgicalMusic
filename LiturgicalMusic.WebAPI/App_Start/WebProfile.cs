using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Web;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.WebAPI.Controllers;
using LiturgicalMusic.Common;
using X.PagedList;

namespace LiturgicalMusic.WebAPI.App_Start
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<IComposer, ComposerController.ComposerModel>().ReverseMap();
            CreateMap<ISong, SongController.SongModel>().ReverseMap();
            CreateMap<IStanza, SongController.StanzaModel>().ReverseMap();
            CreateMap<IInstrumentalPart, SongController.InstrumentalPartModel>().ReverseMap();
            CreateMap<IFilter, SongController.FilterModel>().ReverseMap();
        }
    }
}