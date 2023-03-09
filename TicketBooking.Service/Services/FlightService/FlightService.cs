using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketBooking.Common.AppExceptions;
using TicketBooking.Data.DataModel;
using TicketBooking.Data.Infrastructure;
using AutoMapper;
using Microsoft.Extensions.Logging;
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
        private readonly IFlightScheRepository flightScheRepo;
        private readonly ILogger<Flight> logger;
        public FlightService(IUnitOfWork unitOfWork, IMapper mapper, 
                             IFlightRepository flightRepo, IAircraftRepository aircraftRepo,
                             IAirportService airportService, IFlightScheServices flightScheServices,
                             IFlightScheRepository flightScheRepo, ILogger<Flight> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.flightRepo = flightRepo;
            this.aircraftRepo = aircraftRepo;
            this.airportService = airportService;
            this.flightScheServices = flightScheServices;
            this.flightScheRepo = flightScheRepo;
            this.logger = logger;
        }
        

        public async Task<IEnumerable<FlightViewModel>> GetFlightAsync()
        {
            var flight = await flightRepo.GetAllFlight();
            
            if (flight == null)
            {
                throw new Exception("No flight available now");
            }
            else
            {
                return mapper.Map<IEnumerable<FlightViewModel>>(flight);
            }
        }

        public async Task<IEnumerable<FlightViewModel>> GetFlightAsync(Guid id)
        {
            var flight = await flightRepo.GetFlightById(id);
            return flight == null ? throw new Exception("ID does not exist") : mapper.Map<IEnumerable<FlightViewModel>>(flight);
        }

        public async Task<IEnumerable<FlightViewModel>> GetFlightAsync(FlightRequest flightModel)
        {
            var flight = await flightRepo.GetFlightByRequest(flightModel);
            return flight == null
                ? throw new Exception("None flight match request")
                : mapper.Map<IEnumerable<FlightViewModel>>(flight);
        }

        public async Task<int> UpdateFlightAsync(FlightUpdateModel flightUpdateModel)
        {
            var flight = await flightRepo.GetById(flightUpdateModel.Id);
            var sche = await flightScheRepo.GetById(flightUpdateModel.FlightSche.Id);
            if (flight.IsFlightActive == false)
            {
                return 0;
            }

            flight.Id = flightUpdateModel.Id;
            flight.AircraftId = flightUpdateModel.AircraftId;
            flight.DefaultBaggage = flightUpdateModel.DefaultBaggage;
            flight.BusinessPrice = flightUpdateModel.BusinessPrice;
            flight.EconomyPrice = flightUpdateModel.EconomyPrice;
            sche.Id = flightUpdateModel.FlightSche.Id;
            sche.DepartureTime = flightUpdateModel.FlightSche.DepartureTime;
            sche.ArrivalTime = flightUpdateModel.FlightSche.ArrivalTime;
            
            await flightRepo.Update(flight);
            await flightScheRepo.Update(sche);
            return await unitOfWork.CompletedAsync();
        }

        public async Task<Guid> InsertAsync(FlightRequestModel flightRequestModel)
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
                RemainEconomySeat = tempAircarftModel.NumColumnSeat * tempAircarftModel.NumRowEconomy,
                RemainBusinessSeat = tempAircarftModel.NumColumnSeat * tempAircarftModel.NumRowBusiness,
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
            await unitOfWork.CompletedAsync();
            return flight.Id;
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

        public async Task<bool> UpdateFlightSeat(Guid flightId, SeatClassType type, int number)
        {
            var flight = await flightRepo.GetById(flightId);
            if (type == SeatClassType.Business)
            {
                flight.RemainBusinessSeat = flight.RemainBusinessSeat - number;
                
                if (flight.RemainBusinessSeat < 0)
                { 
                    return false;
                }
            }
            
            if (type == SeatClassType.Economy)
            {
                flight.RemainEconomySeat = flight.RemainEconomySeat - number;
                
                if (flight.RemainEconomySeat < 0)
                {
                    return false;
                }
            }

            flight.RemainingSeat = flight.RemainingSeat - number;
            
            if (flight.RemainingSeat < 0)
            { 
                return false;
            }
            
            
            var modifieldflight = flight; 
            
            var result = await flightRepo.Update(modifieldflight);
            
            return await unitOfWork.CompletedAsync() == 1;
        }
    }
}
