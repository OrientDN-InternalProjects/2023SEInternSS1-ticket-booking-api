using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking.Data.Infrastructure
{
    public interface IRepository<T,X> where T : class where X : new()
    {
        Task<T?> GetById(X id);
        Task<IEnumerable<T>> GetAll(params string[]? includes);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        Task<bool> Add(T entity);
        Task<bool> Remove(X id);
        Task<bool> Update(T entity);
        Task<IEnumerable<T>> GetPagedAdvancedReponseAsync(int pageNumber, int pageSize, string orderBy, string fields);
    }
}
