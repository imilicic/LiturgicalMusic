using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Service.Common;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Repository;
using LiturgicalMusic.Common;
using LiturgicalMusic.Model;
using AutoMapper;
using X.PagedList;

namespace LiturgicalMusic.Service
{
    public class SongService : ISongService
    {
        #region Properties

        /// <summary>
        /// Gets or sets unit of work.
        /// </summary>
        /// <value>The unit of work.</value>
        protected UnitOfWork UnitOfWork { get; private set; }

        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        protected IMapper Mapper { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes new instace of <see cref="SongService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="mapper">The mapper.</param>
        public SongService(UnitOfWork unitOfWork, IMapper mapper)
        {
            this.UnitOfWork = unitOfWork;
            this.Mapper = mapper;
        }
        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates a new song.
        /// </summary>
        /// <returns></returns>
        public ISong Create()
        {
            return new Song();
        }

        /// <summary>
        /// Gets song by ID which contains certain options.
        /// </summary>
        /// <param name="songId">The song ID.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public async Task<ISong> GetByIdAsync(int songId, IOptions options)
        {
            return await UnitOfWork.SongRepository.GetByIdAsync(songId, options);
        }


        /// <summary>
        /// Gets all songs filtered, ordered, using pages
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="options">The options.</param>
        /// <param name="orderBy">The string represeting how to order songs.</param>
        /// <param name="ascending">Whether to order ascending or descending.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns></returns>
        public async Task<IPagedList<ISong>> GetAsync(IFilter filter, IOptions options, string orderBy, bool ascending, int pageNumber, int pageSize)
        {
            return await UnitOfWork.SongRepository.GetAsync(filter, options, orderBy, ascending, pageNumber, pageSize);
        }

        /// <summary>
        /// Inserts a song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
        public async Task<ISong> InsertAsync(ISong song)
        {
            return await UnitOfWork.SongRepository.InsertAsync(song);
        }

        /// <summary>
        /// Makes a preview of a song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
        public async Task<ISong> PreviewAsync(ISong song)
        {
            return await UnitOfWork.SongRepository.PreviewAsync(song);
        }

        /// <summary>
        /// Updates a song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
        public async Task<ISong> UpdateAsync(ISong song)
        {
            // cud stanzas
            IList<IStanza> stanzas = await UnitOfWork.StanzaRepository.GetBySongAsync(song.Id);

            foreach (IStanza stanza in stanzas)
            {
                if (song.Stanzas.SingleOrDefault(s => s.Id.Equals(stanza.Id)) == null) {
                    await UnitOfWork.StanzaRepository.DeleteAsync(stanza.Id);
                }
            }

            foreach (IStanza stanza in song.Stanzas)
            {
                if (stanzas.SingleOrDefault(s => s.Id.Equals(stanza.Id)) == null)
                {
                    await UnitOfWork.StanzaRepository.InsertAsync(stanza, song.Id);
                }
                else
                {
                    await UnitOfWork.StanzaRepository.UpdateAsync(stanza);
                }
            }

            // cud instrumental parts
            IList<IInstrumentalPart> instrumentalParts = await UnitOfWork.InstrumentalPartRepository.GetBySongAsync(song.Id);

            foreach (IInstrumentalPart part in instrumentalParts)
            {
                if (song.InstrumentalParts.SingleOrDefault(p => p.Id.Equals(part.Id)) == null)
                {
                    await UnitOfWork.InstrumentalPartRepository.DeleteAsync(part.Id);
                }
            }

            foreach (IInstrumentalPart part in song.InstrumentalParts)
            {
                if (instrumentalParts.SingleOrDefault(p => p.Id.Equals(part.Id)) == null)
                {
                    await UnitOfWork.InstrumentalPartRepository.InsertAsync(part, song.Id);
                }
                else
                {
                    await UnitOfWork.InstrumentalPartRepository.UpdateAsync(part);
                }
            }

            // update song
            return await UnitOfWork.SongRepository.UpdateAsync(song);
        }
        #endregion Methods
    }
}
