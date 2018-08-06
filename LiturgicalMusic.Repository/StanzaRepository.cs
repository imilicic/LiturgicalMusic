using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.DAL;
using LiturgicalMusic.Repository.Common;
using AutoMapper;
using LiturgicalMusic.Model.Common;
using System.Linq.Expressions;
using System.Data.Entity;

namespace LiturgicalMusic.Repository
{
    public class StanzaRepository : Repository<StanzaEntity>, IStanzaRepository
    {
        #region Properties
        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        protected IMapper Mapper { get; private set; }
        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes new instace of <see cref="StanzaRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="mapper">The mapper.</param>
        public StanzaRepository(MusicContext context, IMapper mapper) : base(context)
        {
            this.Mapper = mapper;
        }
        #endregion Constructors

        #region Methods

        /// <summary>
        /// Deletes stanza by ID.
        /// </summary>
        /// <param name="stanzaId">The stanza ID.</param>
        /// <returns></returns>
        public async Task DeleteAsync(int stanzaId)
        {
            await base.DeleteAsync(await base.GetByIdAsync(stanzaId));
        }

        /// <summary>
        /// Gets all stanzas belonging to a song.
        /// </summary>
        /// <param name="songId">The song ID.</param>
        /// <returns></returns>
        public async Task<IList<IStanza>> GetBySongAsync(int songId)
        {
            IList<StanzaEntity> stanzas = await base.Get(s => s.SongId.Equals(songId)).ToListAsync();

            return Mapper.Map<IList<IStanza>>(stanzas);
        }

        /// <summary>
        /// Inserts a new stanza.
        /// </summary>
        /// <param name="stanza">The stanza.</param>
        /// <param name="songId">The song ID.</param>
        /// <returns></returns>
        public async Task<IStanza> InsertAsync(IStanza stanza, int songId)
        {
            StanzaEntity stanzaDb = Mapper.Map<StanzaEntity>(stanza);

            stanzaDb.SongId = songId;
            stanzaDb = await base.InsertAsync(stanzaDb);

            return Mapper.Map<IStanza>(stanzaDb);
        }

        /// <summary>
        /// Updates stanza.
        /// </summary>
        /// <param name="stanza">The stanza.</param>
        /// <returns></returns>
        public async Task<IStanza> UpdateAsync(IStanza stanza)
        {
            StanzaEntity stanzaDb = await base.GetByIdAsync(stanza.Id);

            Mapper.Map(stanza, stanzaDb);
            stanzaDb = await base.UpdateAsync(stanzaDb);

            return Mapper.Map<IStanza>(stanzaDb);
        }
        #endregion Methods
    }
}