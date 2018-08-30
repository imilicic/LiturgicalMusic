using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Common;
using X.PagedList;

namespace LiturgicalMusic.Service.Common
{
    public interface ISongService
    {
        #region Methods

        /// <summary>
        /// Creates a new song.
        /// </summary>
        /// <returns></returns>
        ISong Create();

        /// <summary>
        /// Deletes a song.
        /// </summary>
        /// <param name="songId">The song ID.</param>
        /// <returns></returns>
        Task DeleteAsync(int songId);

        /// <summary>
        /// Gets song by ID which contains certain options.
        /// </summary>
        /// <param name="songId">The song ID.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<ISong> GetByIdAsync(int songId, string[] options);

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
        Task<IPagedList<ISong>> GetAsync(IFilter filter, string[] options, ISorting sortingOptions, IPaging pageOptions);

        /// <summary>
        /// Inserts a song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
        Task<ISong> InsertAsync(ISong song);

        /// <summary>
        /// Makes a preview of a song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
        Task<ISong> PreviewAsync(ISong song);

        /// <summary>
        /// Updates a song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
        Task<ISong> UpdateAsync(ISong song);
        #endregion Methods
    }
}
