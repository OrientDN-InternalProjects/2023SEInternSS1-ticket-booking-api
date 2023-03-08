using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Data.DataModel;
using TicketBooking.Data.DbContext;
using TicketBooking.Data.Infrastructure;

namespace TicketBooking.Data.Repository
{
    public interface IFlightRepository : IRepository<Flight>
    {
        Task<IEnumerable<Flight>> GetFlightByDate(DateTime date);
        Task<IEnumerable<Flight>> GetFlightByAirport(string departairport, string arrivalairport);
        Task<IEnumerable<Flight>> GetAllFlight();
    }

    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        public FlightRepository(TicketBookingDbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<Flight>> GetAllFlight()
        {

            var query = from f in _context.Flights
                        join fs in _context.FlightSchedules on f.ScheduleId equals fs.Id
                        join ar in _context.Aircrafts on f.AircraftId equals ar.Id
                        select new Flight
                        {
                            Id = f.Id,
                            AircraftId = f.AircraftId,
                            Aircraft = f.Aircraft,
                            TotalSeat = f.TotalSeat,
                            RemainingSeat = f.RemainingSeat,
                            IsFlightActive = f.IsFlightActive,
                            DefaultBaggage = f.DefaultBaggage,
                            BusinessPrice = f.BusinessPrice,
                            EconomyPrice = f.EconomyPrice,
                            ScheduleId = f.ScheduleId,
                            Schedule = new FlightSchedule()
                            {
                                Id = f.Schedule.Id,
                                DepartureAirportId = f.Schedule.DepartureAirportId,
                                AirportDepart = f.Schedule.AirportDepart,
                                ArrivalAirportId = f.Schedule.ArrivalAirportId,
                                AirportArrival = f.Schedule.AirportArrival,
                                DepartureTime = f.Schedule.DepartureTime,
                                ArrivalTime = f.Schedule.ArrivalTime
                            }
                        };
            
            return await query.ToListAsync();
        }
        
        public async Task<IEnumerable<Flight>> GetFlightByDate(DateTime date)
        {
            var convertedDate = date.Date;
            var query = from f in _context.Flights
                        join fs in _context.FlightSchedules on f.ScheduleId equals fs.Id
                        join ar in _context.Aircrafts on f.AircraftId equals ar.Id
                        where (DateTime.Compare(fs.DepartureTime.Date, convertedDate) == 0)
                        select new Flight
                        {
                            Id = f.Id,
                            AircraftId = f.AircraftId,
                            Aircraft = f.Aircraft,
                            TotalSeat = f.TotalSeat,
                            RemainingSeat = f.RemainingSeat,
                            IsFlightActive = f.IsFlightActive,
                            DefaultBaggage = f.DefaultBaggage,
                            BusinessPrice = f.BusinessPrice,
                            EconomyPrice = f.EconomyPrice,
                            ScheduleId = f.ScheduleId,
                            Schedule = new FlightSchedule()
                            {
                                Id = f.Schedule.Id,
                                DepartureAirportId = f.Schedule.DepartureAirportId,
                                AirportDepart = f.Schedule.AirportDepart,
                                ArrivalAirportId = f.Schedule.ArrivalAirportId,
                                AirportArrival = f.Schedule.AirportArrival,
                                DepartureTime = f.Schedule.DepartureTime,
                                ArrivalTime = f.Schedule.ArrivalTime
                            }
                        };

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Flight>> GetFlightByAirport(string departairport, string arrivalairport)
        {
            var departGUID = await (from a in _context.Airports
                            where a.Code == departairport
                            select a.Id).ToListAsync();
            
            var arrivalGUID = await (from a in _context.Airports
                            where a.Code == arrivalairport
                            select a.Id).ToListAsync();

            var query = from f in _context.Flights
                        join fs in _context.FlightSchedules on f.ScheduleId equals fs.Id
                        join ar in _context.Aircrafts on f.AircraftId equals ar.Id
                        where (departGUID.Contains(fs.DepartureAirportId)
                               && arrivalGUID.Contains(fs.ArrivalAirportId))
                        select new Flight
                        {
                            Id = f.Id,
                            AircraftId = f.AircraftId,
                            Aircraft = f.Aircraft,
                            TotalSeat = f.TotalSeat,
                            RemainingSeat = f.RemainingSeat,
                            IsFlightActive = f.IsFlightActive,
                            DefaultBaggage = f.DefaultBaggage,
                            BusinessPrice = f.BusinessPrice,
                            EconomyPrice = f.EconomyPrice,
                            ScheduleId = f.ScheduleId,
                            Schedule = new FlightSchedule()
                            {
                                Id = f.Schedule.Id,
                                DepartureAirportId = f.Schedule.DepartureAirportId,
                                AirportDepart = f.Schedule.AirportDepart,
                                ArrivalAirportId = f.Schedule.ArrivalAirportId,
                                AirportArrival = f.Schedule.AirportArrival,
                                DepartureTime = f.Schedule.DepartureTime,
                                ArrivalTime = f.Schedule.ArrivalTime
                            }
                        };

            return await query.ToListAsync();
        }
    }
}
