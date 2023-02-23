using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.DbContext;

namespace TicketBooking.Data.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private TicketBookingDbContext _Context;
        private IAircraftRepository aircraft;

        public RepositoryWrapper(TicketBookingDbContext context)
        {
            _Context = context;
        }

        public IAircraftRepository _aircrafts
        {
            get
            {
                if (aircraft == null)
                {
                    aircraft = new AircraftRepository(_Context);
                }
                return aircraft;
            }
        }
    }
}
