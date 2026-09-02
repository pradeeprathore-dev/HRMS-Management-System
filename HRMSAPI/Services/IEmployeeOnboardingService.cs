using HRMSAPI.DTOs.EmployeeOnboarding;

namespace HRMSAPI.Services
{
    public interface IEmployeeOnboardingService
    {
        Task<List<EmployeeOnboardingResponseDto>> GetAllAsync();

        Task<EmployeeOnboardingResponseDto?> GetByIdAsync(int id);

        Task<EmployeeOnboardingResponseDto> CreateAsync(EmployeeOnboardingCreateDto dto);

        Task<EmployeeOnboardingResponseDto?> UpdateAsync(EmployeeOnboardingUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}