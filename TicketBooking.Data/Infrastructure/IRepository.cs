using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetById(string id);
        Task<IEnumerable<T>> GetAll(params string[] includes);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        Task<bool> Add(T entity);
        Task<bool> Remove(string id);
        Task<bool> Update(T entity);
        Task<IEnumerable<T>> GetPagedAdvancedReponseAsync(int pageNumber, int pageSize, string orderBy, string fields);
    }
}
