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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public IAircraftRepository _aircrafts;

        public AircraftService(IUnitOfWork unitOfWork, IMapper mapper, IAircraftRepository aircrafts)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _aircrafts = aircrafts;
        }

        public async Task<IEnumerable<AircraftViewModel>> GetAircraftAsync()
        {
            var aircraft = await _aircrafts.GetAll();
            if (aircraft == null)
            {
                throw new Exception("No data to display");
            }
            else
            {
                return _mapper.Map<IEnumerable<AircraftViewModel>>(aircraft);
            }
        }

        public async Task<AircraftViewModel> GetAircraftAsync(Guid id)
        {
            var aircraft = await _aircrafts.GetById(id);
            return aircraft == null ? throw new Exception("ID cannot be found") : _mapper.Map<AircraftViewModel> (aircraft);
        }

        public async Task<int> UpdateAircraftAsync(AircraftViewModel aircraftDto)
        {
            var aircraft = _mapper.Map<Aircraft>(aircraftDto);
            await _aircrafts.Update(aircraft);
            return await _unitOfWork.CompletedAsync();
        }

        public async Task<int> InsertAsync(AircraftViewModel aircraftDto)
        {
            var aircraft = _mapper.Map<Aircraft>(aircraftDto);
            if (aircraft.Model.Length > 6 && aircraft != null)
            {
                throw new Exception("Model name's length cannot be larger than 6");
            }

            else if (aircraft.Manufacture.Length > 10 && aircraft != null)
            {
                throw new Exception("Manufacture name's length cannot be larger than 10");
            }

            else if (aircraft == null)
            {
                throw new Exception("Nothing to add");
            }

            else
            {
                await _aircrafts.Add(aircraft);
                return await _unitOfWork.CompletedAsync();
            }
        }

        public async Task<int> RemoveAsync(Guid id)
        {
            var aircraft = _aircrafts.Find(c => c.Id == id).FirstOrDefault();
            if (aircraft == null)
            {
                throw new Exception("ID is not found");
            }
            else
            {
                await _aircrafts.Remove(aircraft.Id);
                return await _unitOfWork.CompletedAsync();
            }
        }
    }
}