using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.DataModel;
using TicketBooking.Data.Infrastructure;
using TicketBooking.Data.Repository;
using TicketBooking.Model.Models;

namespace TicketBooking.Service.Services.AirportService
{
    public class AirportService : IAirportService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IAirportRepository airportRepo;
        public AirportService(IUnitOfWork unitOfWork, IMapper mapper, IAirportRepository airportRepo)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.airportRepo = airportRepo;
        }

        public async Task<IEnumerable<AirportViewModel>> GetAirportAsync()
        {
            var airport = await airportRepo.GetAll();
            if (airport == null)
            {
                throw new Exception("No data to display");
            }

            else
            {
                return mapper.Map<IEnumerable<AirportViewModel>>(airport);
            }
        }

        public async Task<AirportViewModel> GetAirportAsync(Guid id)
        {
            var airport = await airportRepo.GetById(id);
            return airport == null ? throw new Exception("ID cannot be found") : mapper.Map<AirportViewModel>(airport);
        }

        public async Task<AirportViewModel> GetAirportAsync(string code)
        {
            var airports = await airportRepo.GetAll();

            //Go through all object list return from GetAll() to check the code
            foreach (var airport in airports)
            {
                if (airport.Code == code)
                {
                    var result = new AirportViewModel()
                    {
                        Id = airport.Id,
                        Name = airport.Name,
                        City = airport.City,
                        Country = airport.Country
                    };

                    return result;
                }
            }

            throw new Exception("No airport exist");
        }

        public async Task<int> UpdateAirportAsync(AirportViewModel airportViewModel)
        {
            var airport = mapper.Map<Airport>(airportViewModel);
            await airportRepo.Update(airport);
            return await unitOfWork.CompletedAsync();
        }

        public async Task<int> InsertAsync(AirportViewModel airportViewModel)
        {
            var airport = mapper.Map<Airport>(airportViewModel);
            await airportRepo.Add(airport);
            return await unitOfWork.CompletedAsync();
        }

        public async Task<int> RemoveAsync(Guid id)
        {
            var airport = airportRepo.Find(c => c.Id == id).FirstOrDefault();
            if (airport == null)
            {
                throw new Exception("ID is not found");
            }

            else
            {
                await airportRepo.Remove(airport.Id);
                return await unitOfWork.CompletedAsync();
            }
        }
    }
}
