using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Model
{
    public class DIModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<ICode>().To<Code>();
            Bind<IComposer>().To<Composer>();
            Bind<IInstrumentalPart>().To<InstrumentalPart>();
            Bind<ISong>().To<Song>();
            Bind<IStanza>().To<Stanza>();
        }
    }
}
