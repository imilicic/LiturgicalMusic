using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using LiturgicalMusic.Repository.Common;
using Ninject.Extensions.Factory;

namespace LiturgicalMusic.Repository
{
    public class DIModule : Ninject.Modules.NinjectModule
    {
        #region Methods
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IComposerRepository>().To<ComposerRepository>();
            Bind<ISongRepository>().To<SongRepository>();
            Bind(typeof(IRepository<>)).To(typeof(Repository<>));
            Bind<IUnitOfWorkFactory>().ToFactory();
            Bind<IUnitOfWork>().To<UnitOfWork>();
        }
        #endregion Methods
    }
}
