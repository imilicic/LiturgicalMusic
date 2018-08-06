using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.DAL;
using System.Data.Entity;
using System.Linq.Expressions;
using LiturgicalMusic.Common;
using LiturgicalMusic.Repository.Common;
using X.PagedList;

namespace LiturgicalMusic.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity: class, IEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        internal MusicContext Context;

        /// <summary>
        /// Gets or sets the DbSet.
        /// </summary>
        /// <value>The DbSet.</value>
        internal DbSet<TEntity> DbSet;
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes new instace of <see cref="Repository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public Repository(MusicContext context)
        {
            this.Context = context;
            this.DbSet = Context.Set<TEntity>();
        }
        #endregion Constructors

        #region Methods

        /// <summary>
        /// Deletes entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async virtual Task DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets entities which can be filtered, ordered and include certain properties.
        /// </summary>
        /// <param name="filter">The filter Expression.</param>
        /// <param name="orderBy">The orderBy function.</param>
        /// <param name="includeProperties">The properties.</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        /// <summary>
        /// Gets entity by ID which can include certain properties.
        /// </summary>
        /// <param name="entityId">The entity ID.</param>
        /// <param name="includeProperties">The properties.</param>
        /// <returns></returns>
        public async virtual Task<TEntity> GetByIdAsync(int entityId, string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            foreach (var includeProperty in includeProperties.Split
                   (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.SingleOrDefaultAsync(e => e.Id.Equals(entityId));
        }

        /// <summary>
        /// Inserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async virtual Task<TEntity> InsertAsync(TEntity entity)
        {
            DbSet.Add(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return entity;
        }
        #endregion Methods
    }
}
