using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TicketBooking.Data.DbContext;

namespace TicketBooking.Data.Infrastructure
{
    public class GenericRepository<T, X> : IRepository<T, X> where T : class where X : new()
    {
        protected TicketBookingDbContext Context;
        protected DbSet<T> DbSet;
        public GenericRepository(TicketBookingDbContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }
        public async Task<bool> Add(T entity)
        {
            await DbSet.AddAsync(entity);
            return true;
        }

        public async Task<IEnumerable<T>> GetPagedAdvancedReponseAsync(int pageNumber, int pageSize, string orderBy, string fields)
        {
            return await Context
                 .Set<T>()
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .Select<T>("new(" + fields + ")")
                 .OrderBy(orderBy)
                 .AsNoTracking()
                 .ToListAsync();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return DbSet.Where(expression);
        }

        public async Task<IEnumerable<T>> GetAll(params string[]? includes)
        {
            IQueryable<T> query = DbSet;
            if (includes == null)
            {
                throw new Exception("No data to get");
            }
            if (includes.Length > 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            return query;
        }

        public async Task<T?> GetById(X id, params string[] includes)
        {
            var model = await DbSet.FindAsync(id);
            foreach (var path in includes)
            {
                Context.Entry(model).Reference(path).Load();
            }
            return model;
        }
        
        public async Task<bool> Remove(X id)
        {
            var t = await DbSet.FindAsync(id);
            
               if (t != null)
            {
                DbSet.Remove(t);
                return true;
            }
            else
                return false;
        }
        
        public async Task<bool> Update(T entity)
        {
            this.Context.Entry<T>(entity).State = EntityState.Modified;
            return true;
        }
    }
}
