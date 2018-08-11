using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Repository.Common;
using LiturgicalMusic.DAL;
using LiturgicalMusic.Model.Common;
using AutoMapper;
using System.Data.Entity;

namespace LiturgicalMusic.Repository
{
    public class ComposerRepository : IComposerRepository
    {
        #region Properties
        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        protected IMapper Mapper { get; private set; }

        /// <summary>
        /// Gets or sets the generic repository.
        /// </summary>
        /// <value>The repository.</value>
        protected IRepository<ComposerEntity> Repository { get; private set; }

        /// <summary>
        /// Gets or sets the unit of work class.
        /// </summary>
        /// <value>The unit of work.</value>
        protected IUnitOfWork UnitOfWork { get; private set; }
        #endregion Properties 

        #region Constructors
        /// <summary>
        /// Initializes new instace of <see cref="ComposerRepository"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public ComposerRepository(IMapper mapper, IRepository<ComposerEntity> repository)
        {
            this.Mapper = mapper;
            this.Repository = repository;
            this.UnitOfWork = Repository.CreateUnitOfWork();
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Gets all composers.
        /// </summary>
        /// <returns></returns>
        public async Task<IList<IComposer>> GetAsync()
        {
            IList<ComposerEntity> composers = await Repository.Get().OrderBy(c => c.Surname).ToListAsync();
            return Mapper.Map<IList<IComposer>>(composers);
        }

        /// <summary>
        /// Gets composer by ID.
        /// </summary>
        /// <param name="composerId">The composer ID.</param>
        /// <returns></returns>
        public async Task<IComposer> GetByIdAsync(int composerId)
        {
            ComposerEntity composer = await Repository.Get().SingleOrDefaultAsync(c => c.Id.Equals(composerId));
            return Mapper.Map<IComposer>(composer);
        }

        /// <summary>
        /// Inserts a composer.
        /// </summary>
        /// <param name="composer">The composer.</param>
        /// <returns></returns>
        public async Task<IComposer> InsertAsync(IComposer composer)
        {
            ComposerEntity composerEntity = Mapper.Map<ComposerEntity>(composer);

            composerEntity = await UnitOfWork.InsertAsync(composerEntity);
            await UnitOfWork.CommitAsync();

            return Mapper.Map<IComposer>(composerEntity);
        }
        #endregion Methods
    }
}
