using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.DbContext;

namespace TicketBooking.Data.Infrastructure
{
    public class GenericRepository<T, X> : IRepository<T, X> where T : class where X : Type
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
        
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression);
        }
        
        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbSet.ToListAsync();
        }
        
        public async Task<T?> GetById(X id)
        {
            return await dbSet.FindAsync(id);
        }
        
        public async Task<bool> Remove(X id)
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
            dbSet.Remove(entity);
            return true;
        }
    }
}
