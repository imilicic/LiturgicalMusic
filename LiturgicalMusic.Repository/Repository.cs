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
    public class Repository<TEntity> : IRepository<TEntity> where TEntity: class, IEntity
    {
        internal MusicContext Context;
        internal DbSet<TEntity> DbSet;

        public Repository(MusicContext context)
        {
            this.Context = context;
            this.DbSet = Context.Set<TEntity>();
        }

        public async virtual Task DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async virtual Task<List<TEntity>> GetAsync(
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
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

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

        public async virtual Task<TEntity> InsertAsync(TEntity entity)
        {
            DbSet.Add(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public async virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return entity;
        }
    }
}
