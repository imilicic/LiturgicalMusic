using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Repository.Common
{
    public interface IInstrumentalPartRepository
    {
        #region Methods

        /// <summary>
        /// Deletes instrumental part.
        /// </summary>
        /// <param name="instrumentalPartId">ID of instrumental part.</param>
        /// <returns></returns>
        Task DeleteAsync(int instrumentalPartId);

        /// <summary>
        /// Gets all instrumental parts by song ID.
        /// </summary>
        /// <param name="songId">The song ID.</param>
        /// <returns></returns>
        Task<IList<IInstrumentalPart>> GetBySongAsync(int songId);

        /// <summary>
        /// Inserts a new instrumental part.
        /// </summary>
        /// <param name="instrumentalPart">The instrumental part.</param>
        /// <param name="songId">The song ID.</param>
        /// <returns></returns>
        Task<IInstrumentalPart> InsertAsync(IInstrumentalPart instrumentalPart, int songId);

        /// <summary>
        /// Updates instrumental part.
        /// </summary>
        /// <param name="instrumentalPart">The instrumental part.</param>
        /// <returns></returns>
        Task<IInstrumentalPart> UpdateAsync(IInstrumentalPart instrumentalPart);
        #endregion Methods
    }
}
