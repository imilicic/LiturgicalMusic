using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Common;
using System.Linq.Expressions;

namespace LiturgicalMusic.Repository.Common
{
    public interface IRepository<T> where T : class, IEntity
    {
        #region Methods

        /// <summary>
        /// Creates and returns <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <returns></returns>
        IUnitOfWork CreateUnitOfWork();

        /// <summary>
        /// Gets entities which can be filtered, ordered and include certain properties.
        /// </summary>
        /// <param name="filter">The filter Expression.</param>
        /// <param name="orderBy">The orderBy function.</param>
        /// <param name="includeProperties">The properties.</param>
        /// <returns></returns>
        IQueryable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Gets entity by ID which can include certain properties.
        /// </summary>
        /// <param name="entityId">The entity ID.</param>
        /// <param name="includeProperties">The properties.</param>
        /// <returns></returns>
        Task<T> GetByIdAsync(int entityId, string includeProperties = "");
        #endregion Methods
    }
}
