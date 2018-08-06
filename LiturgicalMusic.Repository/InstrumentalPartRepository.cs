using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.DAL;
using LiturgicalMusic.Model.Common;
using AutoMapper;
using LiturgicalMusic.Repository.Common;
using System.Data.Entity;

namespace LiturgicalMusic.Repository
{
    public class InstrumentalPartRepository : Repository<InstrumentalPartEntity>, IInstrumentalPartRepository
    {
        #region Properties
        /// <summary>
        /// Gets or sets mapper.
        /// </summary>
        /// <value>The mapper.</value>
        protected IMapper Mapper { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="InstrumentalPartRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="mapper">The mapper.</param>
        public InstrumentalPartRepository(MusicContext context, IMapper mapper) : base(context)
        {
            this.Mapper = mapper;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Deletes instrumental part.
        /// </summary>
        /// <param name="instrumentalPartId">ID of instrumental part.</param>
        /// <returns></returns>
        public async Task DeleteAsync(int instrumentalPartId)
        {
            await base.DeleteAsync(await base.GetByIdAsync(instrumentalPartId));
        }

        /// <summary>
        /// Gets all instrumental parts by song ID.
        /// </summary>
        /// <param name="songId">The song ID.</param>
        /// <returns></returns>
        public async Task<IList<IInstrumentalPart>> GetBySongAsync(int songId)
        {
            IList<InstrumentalPartEntity> instrumentalParts = await base.Get(p => p.SongId.Equals(songId)).ToListAsync();

            return Mapper.Map<IList<IInstrumentalPart>>(instrumentalParts);
        }

        /// <summary>
        /// Inserts a new instrumental part.
        /// </summary>
        /// <param name="instrumentalPart">The instrumental part.</param>
        /// <param name="songId">The song ID.</param>
        /// <returns></returns>
        public async Task<IInstrumentalPart> InsertAsync(IInstrumentalPart instrumentalPart, int songId)
        {
            InstrumentalPartEntity instrumentalPartDb = Mapper.Map<InstrumentalPartEntity>(instrumentalPart);

            instrumentalPartDb.SongId = songId;
            instrumentalPartDb = await base.InsertAsync(instrumentalPartDb);

            return Mapper.Map<IInstrumentalPart>(instrumentalPartDb);
        }

        /// <summary>
        /// Updates instrumental part.
        /// </summary>
        /// <param name="instrumentalPart">The instrumental part.</param>
        /// <returns></returns>
        public async Task<IInstrumentalPart> UpdateAsync(IInstrumentalPart instrumentalPart)
        {
            InstrumentalPartEntity instrumentalPartDb = await base.GetByIdAsync(instrumentalPart.Id);

            Mapper.Map(instrumentalPart, instrumentalPartDb);
            instrumentalPartDb = await base.UpdateAsync(instrumentalPartDb);

            return Mapper.Map<IInstrumentalPart>(instrumentalPartDb);
        }
        #endregion Methods
    }
}
