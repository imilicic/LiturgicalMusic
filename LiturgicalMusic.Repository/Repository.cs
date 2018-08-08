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

namespace LiturgicalMusic.Repository
{
    public class Repository<T> : IRepository<T> where T: class, IEntity
    {
        #region Properties
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        protected MusicContext DbContext { private get; set; }

        /// <summary>
        /// Gets or sets the database set.
        /// </summary>
        /// <value>The database set.</value>
        protected DbSet<T> DbSet { get; private set; }

        /// <summary>
        /// Gets or sets factory for unit of work.
        /// </summary>
        /// <value>The factory for unit of work.</value>
        protected IUnitOfWorkFactory UowFactory { private get; set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes new instace of <see cref="Repository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="uowFactory">The factory for unit of work.</param>
        public Repository(MusicContext dbContext, IUnitOfWorkFactory uowFactory)
        {
            this.DbContext = dbContext;
            this.DbSet = DbContext.Set<T>();
            this.UowFactory = uowFactory;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Creates and returns <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <returns></returns>
        public IUnitOfWork CreateUnitOfWork()
        {
            return this.UowFactory.CreateUnitOfWork();
        }

        /// <summary>
        /// Gets entities which can be filtered, ordered and include certain properties.
        /// </summary>
        /// <param name="filter">The filter Expression.</param>
        /// <param name="orderBy">The orderBy function.</param>
        /// <param name="includeProperties">The properties.</param>
        /// <returns></returns>
        public virtual IQueryable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = DbSet;

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
        public async virtual Task<T> GetByIdAsync(int entityId, string includeProperties = "")
        {
            IQueryable<T> query = DbSet;

            foreach (var includeProperty in includeProperties.Split
                   (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.SingleOrDefaultAsync(e => e.Id.Equals(entityId));
        }
        #endregion Methods
    }
}
