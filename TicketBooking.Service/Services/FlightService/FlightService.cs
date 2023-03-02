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

namespace TicketBooking.Service.Services.FlightService
{
    public class FlightService : IFlightService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IFlightRepository flightRepo;
        public FlightService(IUnitOfWork unitOfWork, IMapper mapper, IFlightRepository flightRepo)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.flightRepo = flightRepo;
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

        public async Task<int> UpdateFlightAsync(FlightViewModel flightViewModel)
        {
            var flight = mapper.Map<Flight>(flightViewModel);
            await flightRepo.Update(flight);
            return await unitOfWork.CompletedAsync();
        }

        public async Task<int> InsertAsync(FlightViewModel flightViewModel)
        {
            var flight = mapper.Map<Flight>(flightViewModel);
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
