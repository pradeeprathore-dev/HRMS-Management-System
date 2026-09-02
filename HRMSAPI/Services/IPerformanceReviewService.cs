using HRMSAPI.DTOs;

namespace HRMSAPI.Services.Interfaces
{
    public interface IPerformanceReviewService
    {
        Task<List<PerformanceReviewResponseDto>> GetAll();

        Task<List<PerformanceReviewResponseDto>> GetByEmployeeId(
            int employeeId);

        Task<PerformanceReviewResponseDto?> GetById(
            int id);

        Task<PerformanceReviewResponseDto> Create(
            CreatePerformanceReviewDto dto);

        Task<bool> Update(
            int id,
            UpdatePerformanceReviewDto dto);

        Task<bool> Delete(
            int id);
    }
}