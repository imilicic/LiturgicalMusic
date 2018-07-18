using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using LiturgicalMusic.Service.Common;

namespace LiturgicalMusic.Service
{
    public class DIModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IComposerService>().To<ComposerService>();
            Bind<ISongService>().To<SongService>();
        }
    }
}
