using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Repository.Common
{
    public interface IUnitOfWork : IDisposable
    {
        #region Methods

        /// <summary>
        /// Commits changes made in database.
        /// </summary>
        /// <returns></returns>
        Task<int> CommitAsync();

        /// <summary>
        /// Deletes entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<int> DeleteAsync<T>(T entity) where T : class;

        /// <summary>
        /// Deletes entity by entity ID.
        /// </summary>
        /// <param name="entityID">The entity ID.</param>
        /// <returns></returns>
        Task<int> DeleteAsync<T>(int entityID) where T : class;

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<T> InsertAsync<T>(T entity) where T : class;

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<T> UpdateAsync<T>(T entity) where T : class;
        #endregion Methods
    }
}
