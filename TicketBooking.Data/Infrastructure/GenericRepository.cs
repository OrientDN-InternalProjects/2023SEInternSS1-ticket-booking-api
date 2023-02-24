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
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected TicketBookingDbContext _context;
        protected DbSet<T> dbSet;
        public GenericRepository(TicketBookingDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }
        public async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public async Task<IEnumerable<T>> GetPagedAdvancedReponseAsync(int pageNumber, int pageSize, string orderBy, string fields)
        {
            return await _context
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
            return dbSet.Where(expression);
        }

        public async Task<IEnumerable<T>> GetAll(params string[] includes)
        {
            IQueryable<T> query = dbSet;
            if (includes.Length > 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            return query;
        }

        public async Task<T> GetById(string id, params string[] includes)
        {
            var model = await dbSet.FindAsync(id);
            foreach (var path in includes)
            {
                _context.Entry(model).Reference(path).Load();
            }
            return model;
        }

        public async Task<bool> Remove(string id)
        {
            var t = await dbSet.FindAsync(id);

            if (t != null)
            {
                dbSet.Remove(t);
                return true;
            }
            else
                return false;
        }

        public async Task<bool> Update(T entity)
        {
            this._context.Entry<T>(entity).State = EntityState.Modified;
            return true;
        }

        public async Task<T> GetById(string id)
        {
            return await dbSet.FindAsync(id);
        }
    }
}
