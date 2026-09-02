using Entites.Model;
using HRMSAPI.Data;
using HRMSAPI.DTOs;
using HRMSAPI.Repository.Interfaces;
using HRMSAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _repository;
        private readonly INotificationService _notificationService;

        private readonly HRMSDbContext _context;

        public HolidayService(
     IHolidayRepository repository,
     HRMSDbContext context,
     INotificationService notificationService)
        {
            _repository = repository;
            _context = context;
            _notificationService = notificationService;
        }

        // =========================
        // GET ALL
        // =========================

        public async Task<List<HolidayResponseDto>> GetAll()
        {
            var holidays =
                await _repository.GetAll();

            return holidays.Select(x =>
                new HolidayResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Date = x.Date,
                    Description = x.Description,
                    IsOptional = x.IsOptional
                }).ToList();
        }

        // =========================
        // GET BY ID
        // =========================

        public async Task<HolidayResponseDto?> GetById(
            int id)
        {
            var holiday =
                await _repository.GetById(id);

            if (holiday == null)
                return null;

            return new HolidayResponseDto
            {
                Id = holiday.Id,
                Name = holiday.Name,
                Date = holiday.Date,
                Description = holiday.Description,
                IsOptional = holiday.IsOptional
            };
        }

        // =========================
        // CREATE
        // =========================

        public async Task<HolidayResponseDto> Create(
            CreateHolidayDto dto)
        {
            if (await _repository.Exists(dto.Date))
            {
                throw new Exception(
                    "Holiday already exists on this date.");
            }

            var holiday =
                new Holiday
                {
                    Name = dto.Name,
                    Date = dto.Date,
                    Description = dto.Description,
                    IsOptional = dto.IsOptional
                };

            holiday =
                await _repository.Create(holiday);
            // =========================
            // SEND NOTIFICATION TO ALL EMPLOYEES
            // =========================
            // =========================
            // NOTIFY ALL EMPLOYEES
            // =========================

            var employees =
                await _context.Employees.ToListAsync();

            foreach (var employee in employees)
            {
                await _notificationService.Create(
                    new CreateNotificationDto
                    {
                        EmployeeId = employee.Id,

                        Title = "New Holiday Announced",

                        Message =
                            $"{holiday.Name} has been declared on {holiday.Date:dd-MMM-yyyy}."
                    });
            }

            return new HolidayResponseDto
            {
                Id = holiday.Id,
                Name = holiday.Name,
                Date = holiday.Date,
                Description = holiday.Description,
                IsOptional = holiday.IsOptional
            };
        }

        // =========================
        // UPDATE
        // =========================

        public async Task<bool> Update(
            int id,
            CreateHolidayDto dto)
        {
            var holiday =
                await _repository.GetById(id);

            if (holiday == null)
                return false;

            holiday.Name = dto.Name;
            holiday.Date = dto.Date;
            holiday.Description = dto.Description;
            holiday.IsOptional = dto.IsOptional;

            await _repository.Update(holiday);

            return true;
        }

        // =========================
        // DELETE
        // =========================

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }
    }
}