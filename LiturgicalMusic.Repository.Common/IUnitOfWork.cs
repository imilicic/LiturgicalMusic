using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Common;

namespace LiturgicalMusic.Repository.Common
{
    public interface IUnitOfWork : IDisposable
    {
        #region Methods
        /// <summary>
        /// Clears all entities from local.
        /// </summary>
        /// <typeparam name="T">The type of entity to clear.</typeparam>
        void ClearLocal<T>() where T : class;

        /// <summary>
        /// Commits changes made in database.
        /// </summary>
        /// <returns></returns>
        Task<int> CommitAsync();

        /// <summary>
        /// Deletes entity.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<bool> DeleteAsync<T>(T entity) where T : class;

        /// <summary>
        /// Deletes entity by entity ID.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="entityID">The entity ID.</param>
        /// <returns></returns>
        Task<bool> DeleteAsync<T>(int entityID) where T : class;

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<T> InsertAsync<T>(T entity) where T : class;

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<T> UpdateAsync<T>(T entity) where T : class, IEntity;
        #endregion Methods
    }
}
