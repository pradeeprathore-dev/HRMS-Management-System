using Entites.Model;
using HRMSAPI.Data;
using HRMSAPI.DTOs;
using HRMSAPI.Repository;
using HRMSAPI.Repository.Interfaces;
using HRMSAPI.Services.Interfaces;

namespace HRMSAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;

        private readonly HRMSDbContext _context;

        public NotificationService(
            INotificationRepository repository,
            HRMSDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        // =========================
        // GET ALL
        // =========================

        public async Task<List<NotificationResponseDto>> GetAll()
        {
            var data =
                await _repository.GetAll();

            return data.Select(x =>
                new NotificationResponseDto
                {
                    Id = x.Id,

                    EmployeeId = x.EmployeeId,

                    EmployeeName =
                        x.Employee.FirstName + " " +
                        x.Employee.LastName,

                    Title = x.Title,

                    Message = x.Message,

                    IsRead = x.IsRead,

                    CreatedAt = x.CreatedAt
                }).ToList();
        }

        // =========================
        // GET BY EMPLOYEE
        // =========================

        public async Task<List<NotificationResponseDto>>
            GetByEmployeeId(int employeeId)
        {
            var data =
                await _repository.GetByEmployeeId(employeeId);

            return data.Select(x =>
                new NotificationResponseDto
                {
                    Id = x.Id,

                    EmployeeId = x.EmployeeId,

                    EmployeeName =
                        x.Employee.FirstName + " " +
                        x.Employee.LastName,

                    Title = x.Title,

                    Message = x.Message,

                    IsRead = x.IsRead,

                    CreatedAt = x.CreatedAt
                }).ToList();
        }

        // =========================
        // GET BY ID
        // =========================

        public async Task<NotificationResponseDto?> GetById(
            int id)
        {
            var x =
                await _repository.GetById(id);

            if (x == null)
                return null;

            return new NotificationResponseDto
            {
                Id = x.Id,

                EmployeeId = x.EmployeeId,

                EmployeeName =
                    x.Employee.FirstName + " " +
                    x.Employee.LastName,

                Title = x.Title,

                Message = x.Message,

                IsRead = x.IsRead,

                CreatedAt = x.CreatedAt
            };
        }

        // =========================
        // CREATE
        // =========================

        public async Task<NotificationResponseDto> Create(
            CreateNotificationDto dto)
        {
            var employee =
                await _context.Employees.FindAsync(dto.EmployeeId);

            if (employee == null)
                throw new Exception("Employee not found");

            var notification =
                new Notification
                {
                    EmployeeId = dto.EmployeeId,

                    Title = dto.Title,

                    Message = dto.Message,

                    IsRead = false
                };

            await _repository.Create(notification);

            return new NotificationResponseDto
            {
                Id = notification.Id,

                EmployeeId = notification.EmployeeId,

                EmployeeName =
                    employee.FirstName + " " +
                    employee.LastName,

                Title = notification.Title,

                Message = notification.Message,

                IsRead = notification.IsRead,

                CreatedAt = notification.CreatedAt
            };
        }

        // =========================
        // MARK AS READ
        // =========================

        public async Task<bool> MarkAsRead(
    int id,
    int? employeeId)
        {
            var notification =
                await _repository.GetById(id);

            if (notification == null)
                return false;

            // Employee can modify only own notification
            if (employeeId.HasValue &&
                notification.EmployeeId != employeeId.Value)
            {
                return false;
            }

            notification.IsRead = true;

            await _repository.Update(notification);

            return true;
        }

        // =========================
        // DELETE
        // =========================

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }
        public async Task<int> GetUnreadCount(
    int employeeId)
        {
            return await _repository.GetUnreadCount(employeeId);
        }
        public async Task<List<NotificationResponseDto>> GetLatest(
    int employeeId)
        {
            var data =
                await _repository.GetLatest(employeeId);

            return data.Select(x =>
                new NotificationResponseDto
                {
                    Id = x.Id,

                    EmployeeId = x.EmployeeId,

                    EmployeeName =
                        x.Employee.FirstName + " " +
                        x.Employee.LastName,

                    Title = x.Title,

                    Message = x.Message,

                    IsRead = x.IsRead,

                    CreatedAt = x.CreatedAt
                }).ToList();
        }
        public async Task<BellResponseDto> GetBellData(
    int employeeId)
        {
            var latest =
                await _repository.GetLatest(employeeId);

            var unreadCount =
                await _repository.GetUnreadCount(employeeId);

            return new BellResponseDto
            {
                UnreadCount = unreadCount,

                LatestNotifications =
                    latest.Select(x =>
                        new NotificationResponseDto
                        {
                            Id = x.Id,

                            EmployeeId = x.EmployeeId,

                            EmployeeName =
                                x.Employee.FirstName + " " +
                                x.Employee.LastName,

                            Title = x.Title,

                            Message = x.Message,

                            IsRead = x.IsRead,

                            CreatedAt = x.CreatedAt
                        }).ToList()
            };
        }
    }
}