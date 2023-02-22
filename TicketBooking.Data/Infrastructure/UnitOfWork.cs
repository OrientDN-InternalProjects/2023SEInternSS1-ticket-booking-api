using TicketBooking.Data.DbContext;
using TicketBooking.Data.Repository;

namespace TicketBooking.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TicketBookingDbContext _context;

        public UnitOfWork(TicketBookingDbContext context)
        { _context = context; }

        public IAircraftRepository Aircrafts {get; private set; }

        public async Task<int> CompletedAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

