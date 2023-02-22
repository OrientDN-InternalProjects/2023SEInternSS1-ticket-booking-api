using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketBooking.Data;
using TicketBooking.Data.Infrastructure;
using AutoMapper;

namespace TicketBooking.Service.AircraftService
{
    public class AircraftService : IAircraftSerivce
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AircraftService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AircraftDTO>> GetAircraftAsync()
        {
            var aircraft = await _unitOfWork.Aircrafts.GetAll();
            return _mapper.Map<IEnumerable<AircraftDTO>>(aircraft);
        }

        public async Task<bool> UpdateAircraftAsync(AircraftDTO aircraftDto)
        {
            var aircraft = _mapper.Map<Aircraft>(aircraftDto);
            return await _unitOfWork.Aircrafts.Update(aircraft);
        }

        public async Task<bool> InsertAsync(AircraftDTO aircraftDto)
        {
            var aircraft = _mapper.Map<Aircraft>(aircraftDto);
            return await _unitOfWork.Aircrafts.Add(aircraft);
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var aircraft = _unitOfWork.Aircrafts.Find(c => c.Id == id).FirstOrDefault();
            return await _unitOfWork.Aircrafts.Remove(aircraft.Id);
        }

        public async Task<int> CompleteAsync()
        {
            return await _unitOfWork.CompletedAsync();
        }  
    }
}