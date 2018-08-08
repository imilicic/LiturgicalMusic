using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Service.Common;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Repository.Common;
using LiturgicalMusic.Model;

namespace LiturgicalMusic.Service
{
    public class ComposerService : IComposerService
    {
        #region Properties
        /// <summary>
        /// Gets or sets composer repository.
        /// </summary>
        /// <value>The composer repository.</value>
        protected IComposerRepository ComposerRepository { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="ComposerService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ComposerService(IComposerRepository composerRepository)
        {
            this.ComposerRepository = composerRepository;
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
            return await ComposerRepository.GetAsync();
        }

        /// <summary>
        /// Gets composer by ID.
        /// </summary>
        /// <param name="composerId">The composer ID.</param>
        /// <returns></returns>
        public async Task<IComposer> GetByIdAsync(int composerId)
        {
            return await ComposerRepository.GetByIdAsync(composerId);
        }

        /// <summary>
        /// Inserts a composer.
        /// </summary>
        /// <param name="composer">The composer.</param>
        /// <returns></returns>
        public async Task<IComposer> InsertAsync(IComposer composer)
        {
            return await ComposerRepository.InsertAsync(composer);
        }
        #endregion Methods
    }
}
