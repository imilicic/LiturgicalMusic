using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using LiturgicalMusic.Model.Mapping;

namespace LiturgicalMusic.WebAPI.App_Start
{
    public class AutoMapperConfig : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IMapper>().ToConstant(Initialize());
        }

        private IMapper Initialize()
        {
            Mapper.Initialize(config =>
            {
                config.AddProfile<WebProfile>();
                config.AddProfile<ModelProfile>();
            });

            return Mapper.Instance;
        }
    }
}