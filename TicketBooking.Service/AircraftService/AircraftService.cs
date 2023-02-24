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
        private IAircraftRepository _aircrafts;
        public AircraftService(IUnitOfWork unitOfWork, IMapper mapper, IAircraftRepository aircraft)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _aircrafts = aircraft;
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

        public async Task<AircraftViewModel> GetAircraftAsync(string id)
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
            if (aircraftDto == null)
            {
                throw new Exception("Aircraft Model is empty");
            }

            else
            {
                var aircraft = _mapper.Map<Aircraft>(aircraftDto);
                if (aircraft.Model.Length > 6 || aircraft.Manufacture.Length > 10)
                {
                    throw new Exception("Wrong in the length of Aircraft length or Model");
                }

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
                await _aircrafts.Remove(aircraft.Id.ToString());
                return await _unitOfWork.CompletedAsync();
            }
        }
    }
}