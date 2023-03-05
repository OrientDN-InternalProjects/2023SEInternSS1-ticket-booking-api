using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketBooking.Data.DataModel;
using TicketBooking.Data.Infrastructure;
using AutoMapper;
using TicketBooking.Model.Models;
using TicketBooking.Data.Repository;
using TicketBooking.Data.DbContext;
using TicketBooking.Service.Services.AirportService;
using TicketBooking.Service.Services.FlightScheService;

namespace TicketBooking.Service.Services.FlightService
{
    public class FlightService : IFlightService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IFlightRepository flightRepo;
        private readonly IAircraftRepository aircraftRepo;
        private readonly IAirportService airportService;
        private readonly IFlightScheServices flightScheServices;
        public FlightService(IUnitOfWork unitOfWork, IMapper mapper, 
                             IFlightRepository flightRepo, IAircraftRepository aircraftRepo,
                             IAirportService airportService, IFlightScheServices flightScheServices)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.flightRepo = flightRepo;
            this.aircraftRepo = aircraftRepo;
            this.airportService = airportService;
            this.flightScheServices = flightScheServices;
        }

        public async Task<IEnumerable<FlightViewModel>> GetFlightAsync()
        {
            var flight = await flightRepo.GetAll();

            if (flight == null)
            {
                throw new Exception("No flight available now");
            }
            else
            {
                return mapper.Map<IEnumerable<FlightViewModel>>(flight);
            }
        }

        public async Task<FlightViewModel> GetFlightAsync(Guid id)
        {
            var flight = await flightRepo.GetById(id);
            return flight == null ? throw new Exception("ID does not exist") : mapper.Map<FlightViewModel>(flight);
        }
        
        public async Task<IEnumerable<FlightViewModel>> GetFlightAsync(DateTime date)
        {
            var flight = await flightRepo.GetFlightByDate(date);
            return flight == null
                ? throw new Exception("None flight on that day")
                : mapper.Map<IEnumerable<FlightViewModel>>(flight);
        }
        
        public async Task<int> UpdateFlightAsync(FlightViewModel flightViewModel)
        {
            var flight = mapper.Map<Flight>(flightViewModel);
            await flightRepo.Update(flight);
            return await unitOfWork.CompletedAsync();
        }

        public async Task<int> InsertAsync(FlightRequestModel flightRequestModel)
        {
            var tempAircarftModel = await aircraftRepo.GetById(flightRequestModel.AircraftId);
            if (tempAircarftModel == null)
            {
                throw new Exception("The Aircraft ID" + flightRequestModel.AircraftId  + "does not exist");
            }
            var flight = new Flight()
            {
                Id = Guid.NewGuid(),
                AircraftId = flightRequestModel.AircraftId,
                TotalSeat = tempAircarftModel.NumColumnSeat * tempAircarftModel.NumRowSeat,
                RemainingSeat = tempAircarftModel.NumColumnSeat * tempAircarftModel.NumRowSeat,
                ScheduleId = Guid.NewGuid(),
                DefaultBaggage = flightRequestModel.DefaultBaggage,
                IsFlightActive = flightRequestModel.IsFlightActive,
                BusinessPrice = flightRequestModel.BusinessPrice,
                EconomyPrice = flightRequestModel.EconomyPrice
            };

            if (flightRequestModel.DepartTime == null || flightRequestModel.ArrivalTime == null)
            {
                throw new Exception("Depart and Arrival Time cannot be null");
            }
            else
            {
                var departCode = await airportService.GetAirportAsync(flightRequestModel.DepartAirportCode);
                var arrivalCode = await airportService.GetAirportAsync(flightRequestModel.ArrivalAirportCode);

                var flightScheRequestModel = new FlightScheViewModel()
                {
                    Id = flight.ScheduleId,
                    DepartureAirpotId = departCode.Id,
                    ArrivalAirportId = arrivalCode.Id,
                    DepartureAirportCode = departCode.Code,
                    ArrivalAirportCode = arrivalCode.Code,
                    DepartureTime = flightRequestModel.DepartTime,
                    ArrivalTime = flightRequestModel.ArrivalTime
                };

                await flightScheServices.InsertAsync(flightScheRequestModel);
            }


            await flightRepo.Add(flight);
            return await unitOfWork.CompletedAsync();
        }

        public async Task<int> RemoveAsync(Guid id)
        {
            var flight = flightRepo.Find(c => c.Id == id).FirstOrDefault();
            if(flight == null)
            {
                throw new Exception("ID is not found");
            }
            
            else
            {
                await flightRepo.Remove(flight.Id);
                return await unitOfWork.CompletedAsync();
            }    
        }
    }
}
