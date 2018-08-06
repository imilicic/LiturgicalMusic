using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using LiturgicalMusic.Repository.Common;

namespace LiturgicalMusic.Repository
{
    public class DIModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            //Bind<IComposerRepository>().To<ComposerRepository>();
            //Bind<ISongRepository>().To<SongRepository>();
            //Bind<IStanzaRepository>().To<StanzaRepository>();
            //Bind<IInstrumentalPartRepository>().To<InstrumentalPartRepository>();
            Bind<UnitOfWork>().ToSelf().InSingletonScope();
        }
    }
}
