using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Web;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.WebAPI.Controllers;

namespace LiturgicalMusic.WebAPI.App_Start
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<IComposer, ComposerController.ComposerModel>().ReverseMap();
        }
    }
}