using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Service.Common
{
    public interface IComposerService
    {
        #region Methods

        /// <summary>
        /// Creates a new composer.
        /// </summary>
        /// <returns></returns>
        IComposer Create();

        /// <summary>
        /// Gets all composers.
        /// </summary>
        /// <returns></returns>
        Task<IList<IComposer>> GetAsync();

        /// <summary>
        /// Gets composer by ID.
        /// </summary>
        /// <param name="composerId">The composer ID.</param>
        /// <returns></returns>
        Task<IComposer> GetByIdAsync(int composerId);

        /// <summary>
        /// Inserts a composer.
        /// </summary>
        /// <param name="composer">The composer.</param>
        /// <returns></returns>
        Task<IComposer> InsertAsync(IComposer composer);
        #endregion Methods
    }
}
