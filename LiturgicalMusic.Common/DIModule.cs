using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace LiturgicalMusic.Common
{
    public class DIModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IFilter>().To<Filter>();
            Bind<IOptions>().To<Options>();
            Bind<IPaging>().To<Paging>();
            Bind<ISorting>().To<Sorting>();
        }
    }
}
