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

        protected virtual IQueryable<T> GetQueryable(string include = "")
        {
            IQueryable<T> query = DbSet;

            foreach (var includeProperty in include.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        /// <summary>
        /// Gets entities.
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> Get(string include = "")
        {
            return GetQueryable(include);
        }

        /// <summary>
        /// Gets entity by Id.
        /// </summary>
        /// <param name="entityId">The entity ID.</param>
        /// <returns></returns>
        public virtual Task<E> GetById<E>(int entityId) where E : class, IEntity
        {
            return DbContext.Set<E>().SingleOrDefaultAsync(e => e.Id.Equals(entityId));
        }

        /// <summary>
        /// Gets entity by Id.
        /// </summary>
        /// <param name="entityId">The entity ID.</param>
        /// <returns></returns>
        public virtual Task<T> GetById(int entityId, string include = "")
        {
            return GetQueryable(include).SingleOrDefaultAsync(e => e.Id.Equals(entityId));
        }
        #endregion Methods
    }
}
