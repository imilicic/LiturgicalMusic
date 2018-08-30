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
        /// Gets entities.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Get(string[] include = null);

        /// <summary>
        /// Gets entity by Id.
        /// </summary>
        /// <param name="entityId">The entity ID.</param>
        /// <returns></returns>
        Task<E> GetById<E>(int entityId) where E : class, IEntity;

        /// <summary>
        /// Gets entity by Id.
        /// </summary>
        /// <param name="entityId">The entity ID.</param>
        /// <returns></returns>
        Task<T> GetById(int entityId, string[] include = null);
        #endregion Methods
    }
}
