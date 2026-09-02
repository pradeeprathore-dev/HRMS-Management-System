using Entites.Model;
using HRMSAPI.DTOs;

namespace HRMSAPI.Repository
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAll();

        Task<List<Notification>> GetByEmployeeId(
            int employeeId);

        Task<Notification?> GetById(
            int id);

        Task<Notification> Create(
            Notification notification);

        Task<Notification> Update(
            Notification notification);

        Task<bool> Delete(
            int id);
        Task<int> GetUnreadCount(int employeeId);
        Task<List<Notification>> GetLatest(int employeeId);
    }
}
