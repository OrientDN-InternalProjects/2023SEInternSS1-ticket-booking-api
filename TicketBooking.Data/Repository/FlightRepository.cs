using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Data.DataModel;
using TicketBooking.Data.DbContext;
using TicketBooking.Data.Infrastructure;
using TicketBooking.Model.Models;

namespace TicketBooking.Data.Repository
{
    public interface IFlightRepository : IRepository<Flight>
    {
        Task<IEnumerable<Flight>> GetAllFlight();

        Task<IEnumerable<Flight>> GetFlightById(Guid Id);
        
        Task<IEnumerable<Flight>> GetFlightByRequest(FlightRequest flightrequest);
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
                            RemainBusinessSeat = f.RemainBusinessSeat,
                            RemainEconomySeat = f.RemainEconomySeat,
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
        
        public async Task <IEnumerable<Flight>> GetFlightByRequest(FlightRequest flightrequest)
        {
            var convertedDate = flightrequest.DepartDate.Date;
            
            var departGUID = await (from a in _context.Airports
                where a.Code == flightrequest.DepartCode
                select a.Id).ToListAsync();
            
            var arrivalGUID = await (from a in _context.Airports
                where a.Code == flightrequest.ArrivalCode
                select a.Id).ToListAsync();

            var query = from f in _context.Flights
                join fs in _context.FlightSchedules on f.ScheduleId equals fs.Id
                join ar in _context.Aircrafts on f.AircraftId equals ar.Id
                where ((DateTime.Compare(fs.DepartureTime.Date, convertedDate) == 0)
                       && (departGUID.Contains(fs.DepartureAirportId))
                           && (arrivalGUID.Contains(fs.ArrivalAirportId)))
                select new Flight
                {
                    Id = f.Id,
                    AircraftId = f.AircraftId,
                    Aircraft = f.Aircraft,
                    TotalSeat = f.TotalSeat,
                    RemainingSeat = f.RemainingSeat,
                    RemainBusinessSeat = f.RemainBusinessSeat,
                    RemainEconomySeat = f.RemainEconomySeat,
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

        public async Task<IEnumerable<Flight>> GetFlightById(Guid Id)
        {
            var query = from f in _context.Flights
                join fs in _context.FlightSchedules on f.ScheduleId equals fs.Id
                join ar in _context.Aircrafts on f.AircraftId equals ar.Id
                where (f.Id == Id)
                select new Flight
                {
                    Id = f.Id,
                    AircraftId = f.AircraftId,
                    Aircraft = f.Aircraft,
                    TotalSeat = f.TotalSeat,
                    RemainingSeat = f.RemainingSeat,
                    RemainBusinessSeat = f.RemainBusinessSeat,
                    RemainEconomySeat = f.RemainEconomySeat,
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
