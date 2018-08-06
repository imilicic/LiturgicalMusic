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
            Bind<UnitOfWork>().ToSelf().InSingletonScope();
        }
    }
}
