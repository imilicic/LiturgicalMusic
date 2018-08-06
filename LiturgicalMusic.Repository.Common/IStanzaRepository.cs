using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;
using System.Linq.Expressions;

namespace LiturgicalMusic.Repository.Common
{
    public interface IStanzaRepository
    {
        #region Methods

        /// <summary>
        /// Deletes stanza by ID.
        /// </summary>
        /// <param name="stanzaId">The stanza ID.</param>
        /// <returns></returns>
        Task DeleteAsync(int stanzaId);

        /// <summary>
        /// Gets all stanzas belonging to a song.
        /// </summary>
        /// <param name="songId">The song ID.</param>
        /// <returns></returns>
        Task<IList<IStanza>> GetBySongAsync(int songId);

        /// <summary>
        /// Inserts a new stanza.
        /// </summary>
        /// <param name="stanza">The stanza.</param>
        /// <param name="songId">The song ID.</param>
        /// <returns></returns>
        Task<IStanza> InsertAsync(IStanza stanza, int songId);

        /// <summary>
        /// Updates stanza.
        /// </summary>
        /// <param name="stanza">The stanza.</param>
        /// <returns></returns>
        Task<IStanza> UpdateAsync(IStanza stanza);
        #endregion Methods
    }
}
