using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Core.Models;
using System.Data.Entity;
using System.Configuration;


namespace Core.Helpers
{
    
    public interface IRepository<T> where T : class
    {
        IQueryable<T> RetrieveAll();
        IQueryable<T> SearchFor(Expression<Func<T, bool>> where = null);
        IEnumerable<T> Retrieve(Expression<Func<T, bool>> where = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes);        
        T RetrieveById(object id);
        T SingleOrDefault(Expression<Func<T, bool>> cond);

        void Add(T entity);
        void Delete(object id);
        void Update(T entity);
        void Save();
    }

    public static partial class DB
    {
        public class Repository<T> : IRepository<T> where T : class
        {
            /* PROPERTY */
            internal DbContext context;
            internal DbSet<T> dbSet;


            /* CONSTRUCTOR*/
            public Repository(DbContext context)
            {
                this.context = context;
                this.dbSet = context.Set<T>();
            }

            /* INTERFACE IMPLEMENTATION */

            /* get all implementation */
            public virtual IQueryable<T> RetrieveAll()
            {
                IQueryable<T> query = dbSet;
                if (query != null)
                {
                    return query;
                }
                return null;
            }

            /* search for implementation */
            public virtual IQueryable<T> SearchFor(Expression<Func<T, bool>> cond = null)
            {
                IQueryable<T> query = dbSet;
                if (query != null)
                {
                    if (cond != null)
                    {
                        query = query.Where(cond);
                    }

                    return query;
                }
                return null;
            }

            /* retrieve implementation */
            public virtual IEnumerable<T> Retrieve(Expression<Func<T, bool>> cond = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
            {
                IQueryable<T> query = dbSet;
                if (query != null)
                {
                    if (cond != null)
                    {
                        query = query.Where(cond);
                    }


                    if (orderBy != null)
                    {
                        query = orderBy(query);
                    }

                    if (includes != null)
                    {
                        query = includes.Aggregate(query,
                            (current, include) => current.Include(include));
                    }

                    return query.ToList();
                }
                return null;
            }


            /* find implementation */
            public virtual T RetrieveById(object id)
            {
                return dbSet.Find(id);
            }

            /* single or default implementation */
            public virtual T SingleOrDefault(Expression<Func<T, bool>> cond)
            {
                return dbSet.SingleOrDefault(cond);
            }

            /* add implementation */
            public virtual void Add(T entity)
            {
                dbSet.Add(entity);
            }

            /* delete implementation */
            public virtual void Delete(object id)
            {
                T entity = dbSet.Find(id);
                Delete(entity);
            }
            public virtual void Delete(T entity)
            {
                if (context.Entry(entity).State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                }
                dbSet.Remove(entity);
            }

            /* update implementation */
            public void Update(T entity)
            {
                dbSet.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            }

            public void Save()
            {
                context.SaveChanges();
            }

            /* DISPOSING */
            private bool disposed = false;

            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        context.Dispose();
                    }
                }

                this.disposed = true;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
    }
}
