using HRMSAPI.DTOs;

namespace HRMSAPI.Services
{
    public interface INotificationService
    {
        Task<List<NotificationResponseDto>> GetAll();

        Task<List<NotificationResponseDto>> GetByEmployeeId(int employeeId);

        Task<NotificationResponseDto?> GetById(int id);

        Task<NotificationResponseDto> Create(CreateNotificationDto dto);

        Task<bool> MarkAsRead(int id, int? employeeId);

        Task<bool> Delete(int id);
        Task<int> GetUnreadCount(int employeeId);
        Task<List<NotificationResponseDto>> GetLatest(int employeeId);
        Task<BellResponseDto> GetBellData(int employeeId);
    }
}
