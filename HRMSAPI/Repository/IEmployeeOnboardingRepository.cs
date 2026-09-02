using Entites.Model;

namespace HRMSAPI.Repository
{
    public interface IEmployeeOnboardingRepository
    {
        Task<List<EmployeeOnboarding>> GetAllAsync();

        Task<EmployeeOnboarding?> GetByIdAsync(int id);

        Task<EmployeeOnboarding?> GetByEmployeeIdAsync(int employeeId);

        Task<EmployeeOnboarding> CreateAsync(EmployeeOnboarding onboarding);

        Task<EmployeeOnboarding?> UpdateAsync(EmployeeOnboarding onboarding);

        Task<bool> DeleteAsync(int id);

        Task<bool> EmployeeExistsAsync(int employeeId);

        Task<bool> UserExistsAsync(int employeeId);

        Task<bool> DocumentsUploadedAsync(int employeeId);

        Task<bool> LaptopAllocatedAsync(int employeeId);

        Task<bool> ProjectAssignedAsync(int employeeId);
    }
}