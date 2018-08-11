using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Service.Common;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Repository.Common;
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
        /// Gets or sets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        protected IMapper Mapper { get; private set; }

        /// <summary>
        /// Gets or sets the song repository.
        /// </summary>
        /// <value>The song repository.</value>
        protected ISongRepository SongRepository { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes new instace of <see cref="SongService"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="songRepository">The song repository.</param>
        public SongService(IMapper mapper, ISongRepository songRepository)
        {
            this.Mapper = mapper;
            this.SongRepository = songRepository;
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
        /// Deletes a song.
        /// </summary>
        /// <param name="songId">The song ID.</param>
        /// <returns></returns>
        public async Task DeleteAsync(int songId)
        {
            await SongRepository.DeleteAsync(songId);
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
        public async Task<IPagedList<ISong>> GetAsync(IFilter filter, IOptions options, ISorting sortingOptions, IPaging pageOptions)
        {
            return await SongRepository.GetAsync(filter, options, sortingOptions, pageOptions);
        }

        /// <summary>
        /// Gets song by ID which contains certain options.
        /// </summary>
        /// <param name="songId">The song ID.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public async Task<ISong> GetByIdAsync(int songId, IOptions options)
        {
            return await SongRepository.GetByIdAsync(songId, options);
        }

        /// <summary>
        /// Inserts a song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
        public async Task<ISong> InsertAsync(ISong song)
        {
            ISong result = await SongRepository.InsertAsync(song);

            return result;
        }

        /// <summary>
        /// Makes a preview of a song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
        public async Task<ISong> PreviewAsync(ISong song)
        {
            return await SongRepository.PreviewAsync(song);
        }

        /// <summary>
        /// Updates a song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
        public async Task<ISong> UpdateAsync(ISong song)
        {
            return await SongRepository.UpdateAsync(song);
        }
        #endregion Methods
    }
}
