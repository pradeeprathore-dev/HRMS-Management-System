using AutoMapper;
using Entites.Model;
using Helpers;
using HRMSAPI.DTOs;
using HRMSAPI.Repository.Interfaces;
using HRMSAPI.Services.Interfaces;

namespace HRMSAPI.Services
{
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _repository;
        private readonly IMapper _mapper;

        public ShiftService(
            IShiftRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================

        public async Task<List<ShiftResponseDto>> GetAll(PaginationDto paginationDto)
        {
            var shifts = await _repository.GetAll(paginationDto);

            return _mapper.Map<List<ShiftResponseDto>>(shifts);
        }

        // =========================
        // GET BY ID
        // =========================

        public async Task<ShiftResponseDto?> GetById(int id)
        {
            var shift = await _repository.GetById(id);

            if (shift == null)
                return null;

            return _mapper.Map<ShiftResponseDto>(shift);
        }

        // =========================
        // CREATE
        // =========================

        public async Task<ShiftResponseDto> Create(CreateShiftDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ShiftName))
                throw new Exception("Shift Name is required.");

            if (await _repository.IsShiftNameExists(dto.ShiftName))
                throw new Exception("Shift Name already exists.");

            if (dto.StartTime >= dto.EndTime)
                throw new Exception("Shift End Time must be greater than Start Time.");

            if (dto.GraceMinutes < 0 || dto.GraceMinutes > 60)
                throw new Exception("Grace Minutes must be between 0 and 60.");

            if (dto.HalfDayTime <= dto.StartTime)
                throw new Exception("Half Day Time must be after Shift Start Time.");

            if (dto.HalfDayTime >= dto.EndTime)
                throw new Exception("Half Day Time must be before Shift End Time.");

            var shift = _mapper.Map<Shift>(dto);

            var result = await _repository.Create(shift);

            return _mapper.Map<ShiftResponseDto>(result);
        }

        // =========================
        // UPDATE
        // =========================

        public async Task<bool> Update(int id, UpdateShiftDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ShiftName))
                throw new Exception("Shift Name is required.");

            if (await _repository.IsShiftNameExists(dto.ShiftName, id))
                throw new Exception("Shift Name already exists.");

            if (dto.StartTime >= dto.EndTime)
                throw new Exception("Shift End Time must be greater than Start Time.");

            if (dto.GraceMinutes < 0 || dto.GraceMinutes > 60)
                throw new Exception("Grace Minutes must be between 0 and 60.");

            if (dto.HalfDayTime <= dto.StartTime)
                throw new Exception("Half Day Time must be after Shift Start Time.");

            if (dto.HalfDayTime >= dto.EndTime)
                throw new Exception("Half Day Time must be before Shift End Time.");

            var shift = _mapper.Map<Shift>(dto);

            shift.Id = id;

            return await _repository.Update(shift);
        }

        // =========================
        // DELETE
        // =========================

        public async Task<bool> Delete(int id)
        {
            if (await _repository.HasEmployees(id))
                throw new Exception("Cannot delete shift because employees are assigned to it.");

            return await _repository.Delete(id);
        }
        // =========================
        // DROPDOWN
        // =========================

        public async Task<List<ShiftDropdownDto>> GetAllForDropdown()
        {
            var shifts = await _repository.GetAllForDropdown();

            return _mapper.Map<List<ShiftDropdownDto>>(shifts);
        }
    }
}