using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Repository.Common;
using LiturgicalMusic.DAL;
using AutoMapper;
using System.Transactions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace LiturgicalMusic.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Properties
        protected MusicContext DbContext { get; private set; }
        private bool disposed = false;
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="mapper">The mapper.</param>
        public UnitOfWork(MusicContext dbContext)
        {
            this.DbContext = dbContext;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Commits changes made in database.
        /// </summary>
        /// <returns></returns>
        public async Task<int> CommitAsync()
        {
            int result = 0;

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await DbContext.SaveChangesAsync();
                scope.Complete();
            }

            return result;
        }

        /// <summary>
        /// Deletes entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public Task<int> DeleteAsync<T>(T entity) where T : class
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbContext.Set<T>().Attach(entity);
                DbContext.Set<T>().Remove(entity);
            }

            return Task.FromResult(1);
        }

        /// <summary>
        /// Deletes entity by entity ID.
        /// </summary>
        /// <param name="entityID">The entity ID.</param>
        /// <returns></returns>
        public Task<int> DeleteAsync<T>(int entityID) where T : class
        {
            T entity = DbContext.Set<T>().Find(entityID);

            if (entity == null)
            {
                return Task.FromResult(0);
            }

            return DeleteAsync<T>(entity);
        }

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public Task<T> InsertAsync<T>(T entity) where T : class
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbContext.Set<T>().Add(entity);
            }

            return Task.FromResult(entity);
        }

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public Task<T> UpdateAsync<T>(T entity) where T : class
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbContext.Set<T>().Attach(entity);
            }

            dbEntityEntry.State = EntityState.Modified;

            return Task.FromResult(entity);
        }

        /// <summary>
        /// Runs Dispose method.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposed)
            {
                DbContext.Dispose();
            }
            this.disposed = true;
            GC.SuppressFinalize(this);
        }
        #endregion Methods
    }
}
