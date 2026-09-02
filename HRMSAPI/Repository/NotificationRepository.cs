using Entites.Model;
using HRMSAPI.Data;
using HRMSAPI.DTOs;
using HRMSAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly HRMSDbContext _context;

        public NotificationRepository(
            HRMSDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL
        // =========================

        public async Task<List<Notification>> GetAll()
        {
            return await _context.Notifications
                .Include(x => x.Employee)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        // =========================
        // GET BY EMPLOYEE
        // =========================

        public async Task<List<Notification>> GetByEmployeeId(
            int employeeId)
        {
            return await _context.Notifications
                .Include(x => x.Employee)
                .Where(x => x.EmployeeId == employeeId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        // =========================
        // GET BY ID
        // =========================

        public async Task<Notification?> GetById(
            int id)
        {
            return await _context.Notifications
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // =========================
        // CREATE
        // =========================

        public async Task<Notification> Create(
            Notification notification)
        {
            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();

            return notification;
        }

        // =========================
        // UPDATE
        // =========================

        public async Task<Notification> Update(
            Notification notification)
        {
            _context.Notifications.Update(notification);

            await _context.SaveChangesAsync();

            return notification;
        }

        // =========================
        // DELETE
        // =========================

        public async Task<bool> Delete(
            int id)
        {
            var notification =
                await _context.Notifications.FindAsync(id);

            if (notification == null)
                return false;

            _context.Notifications.Remove(notification);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<int> GetUnreadCount(
    int employeeId)
        {
            return await _context.Notifications
                .CountAsync(x =>
                    x.EmployeeId == employeeId &&
                    x.IsRead == false);
        }
        public async Task<List<Notification>> GetLatest(int employeeId)
        {
            return await _context.Notifications
                .Include(x => x.Employee)
                .Where(x => x.EmployeeId == employeeId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .ToListAsync();
        }
        //    public async Task<List<NotificationResponseDto>> GetLatest(
        //int employeeId)
        //    {
        //        var data =
        //            await _repository.GetLatest(employeeId);

        //        return data.Select(x =>
        //            new NotificationResponseDto
        //            {
        //                Id = x.Id,

        //                EmployeeId = x.EmployeeId,

        //                Title = x.Title,

        //                Message = x.Message,

        //                IsRead = x.IsRead
        //            }).ToList();
        //    }

    }
}