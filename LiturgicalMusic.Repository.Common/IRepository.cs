using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Common;
using System.Linq.Expressions;

namespace LiturgicalMusic.Repository.Common
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        #region Methods
        /// <summary>
        /// Deletes entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Gets entities which can be filtered, ordered and include certain properties.
        /// </summary>
        /// <param name="filter">The filter Expression.</param>
        /// <param name="orderBy">The orderBy function.</param>
        /// <param name="includeProperties">The properties.</param>
        /// <returns></returns>
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Gets entity by ID which can include certain properties.
        /// </summary>
        /// <param name="entityId">The entity ID.</param>
        /// <param name="includeProperties">The properties.</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(int entityId, string includeProperties = "");

        /// <summary>
        /// Inserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity entity);
        #endregion Methods
    }
}
