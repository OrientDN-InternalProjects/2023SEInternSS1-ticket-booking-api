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


namespace TicketBooking.Service.Services.FlightScheService
{
    public class FlightScheServices : IFlightScheServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IFlightScheRepository flightScheRepo;
        public FlightScheServices(IUnitOfWork unitOfWork, IMapper mapper, IFlightScheRepository flightScheRepo)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.flightScheRepo = flightScheRepo;
        }

        public async Task<IEnumerable<FlightScheViewModel>> GetFlightScheAsync()
        {
            var flightsche = await flightScheRepo.GetAll();
            if (flightsche == null)
            {
                throw new Exception("No data to display");
            }

            else
            {
                return mapper.Map<IEnumerable<FlightScheViewModel>>(flightsche);
            }
        }

        public async Task<FlightScheViewModel> GetFlightScheAsync(Guid id)
        {
            var flightsche = await flightScheRepo.GetById(id);
            return flightsche == null ? throw new Exception("ID cannot be found") : mapper.Map<FlightScheViewModel>(flightsche);
        }

        public async Task<int> UpdateFlightScheAsync(FlightScheViewModel flightScheViewModel)
        {
            var flightsche = mapper.Map<FlightSchedule>(flightScheViewModel);
            await flightScheRepo.Update(flightsche);
            return await unitOfWork.CompletedAsync();
        }

        public async Task<int> InsertAsync(FlightScheViewModel flightScheViewModel)
        {
            var flightsche = mapper.Map<FlightSchedule>(flightScheViewModel);
            await flightScheRepo.Add(flightsche);
            return await unitOfWork.CompletedAsync();
        }

        public async Task<int> RemoveAsync(Guid id)
        {
            var flightsche = flightScheRepo.Find(c => c.Id == id).FirstOrDefault();
            if (flightsche == null)
            {
                throw new Exception("ID is not found");
            }

            else
            {
                await flightScheRepo.Remove(flightsche.Id);
                return await unitOfWork.CompletedAsync();
            }
        }

    }
}
