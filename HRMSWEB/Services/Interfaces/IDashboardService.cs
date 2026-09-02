using HRMSWEB.Models;

namespace HRMSWEB.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardPageViewModel> GetDashboardDataAsync(string token);
    }
}