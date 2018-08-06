using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Service.Common;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Repository;
using LiturgicalMusic.Model;

namespace LiturgicalMusic.Service
{
    public class ComposerService : IComposerService
    {
        #region Properties
        /// <summary>
        /// Gets or sets unit of work.
        /// </summary>
        /// <value>The unit of work.</value>
        protected UnitOfWork UnitOfWork { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="ComposerService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ComposerService(UnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }
        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates a new composer.
        /// </summary>
        /// <returns></returns>
        public IComposer Create()
        {
            return new Composer();
        }

        /// <summary>
        /// Gets all composers.
        /// </summary>
        /// <returns></returns>
        public async Task<IList<IComposer>> GetAsync()
        {
            return await UnitOfWork.ComposerRepository.GetAsync();
        }

        /// <summary>
        /// Gets composer by ID.
        /// </summary>
        /// <param name="composerId">The composer ID.</param>
        /// <returns></returns>
        public async Task<IComposer> GetByIdAsync(int composerId)
        {
            return await UnitOfWork.ComposerRepository.GetByIdAsync(composerId);
        }

        /// <summary>
        /// Inserts a composer.
        /// </summary>
        /// <param name="composer">The composer.</param>
        /// <returns></returns>
        public async Task<IComposer> InsertAsync(IComposer composer)
        {
            return await UnitOfWork.ComposerRepository.InsertAsync(composer);
        }
        #endregion Methods
    }
}
