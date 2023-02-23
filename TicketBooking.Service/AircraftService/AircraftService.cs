using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketBooking.Data;
using TicketBooking.Data.Infrastructure;
using AutoMapper;
using TicketBooking.Service.Model;
using TicketBooking.Data.Repository;
using TicketBooking.Data.DbContext;

namespace TicketBooking.Service.AircraftService
{
    public class AircraftService : IAircraftSerivce
    {
        private IAircraftRepository Aircrafts;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AircraftService(IUnitOfWork unitOfWork, IMapper mapper, TicketBookingDbContext _context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Aircrafts = new AircraftRepository(_context);
        }

        public async Task<IEnumerable<AircraftDTO>> GetAircraftAsync()
        {
            var aircraft = await Aircrafts.GetAll();
            if (aircraft == null)
            {
                throw new Exception("No data to display");
            }
            else
            {
                return _mapper.Map<IEnumerable<AircraftDTO>>(aircraft);
            }
        }

        public async Task<AircraftDTO> GetAircraftAsync(Guid id)
        {
            var aircraft = await Aircrafts.GetById(id);
            return aircraft == null ? throw new Exception("ID cannot be found") : _mapper.Map<AircraftDTO> (aircraft);
        }

        public async Task<bool> UpdateAircraftAsync(AircraftDTO aircraftDto)
        {
            var aircraft = _mapper.Map<Aircraft>(aircraftDto);
            return await Aircrafts.Update(aircraft);
        }

        public async Task<int> InsertAsync(AircraftDTO aircraftDto)
        {
            var aircraft = _mapper.Map<Aircraft>(aircraftDto);
            await Aircrafts.Add(aircraft);
            return await _unitOfWork.CompletedAsync();
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var aircraft = Aircrafts.Find(c => c.Id == id).FirstOrDefault();
            return aircraft == null ? throw new Exception("ID is not found") : await Aircrafts.Remove(aircraft.Id);

        }

        public async Task<int> CompleteAsync()
        {
            return await _unitOfWork.CompletedAsync();
        }  
    }
}