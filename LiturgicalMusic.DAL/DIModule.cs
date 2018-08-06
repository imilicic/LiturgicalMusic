using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.DAL
{
    public class DIModule : Ninject.Modules.NinjectModule
    {
        #region Methods

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<MusicContext>().ToSelf();
        }
        #endregion Methods
    }
}
