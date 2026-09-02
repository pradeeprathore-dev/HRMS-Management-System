using Entites.Model;

namespace HRMSAPI.Repository.Interfaces
{
    public interface IPerformanceReviewRepository
    {
        Task<List<PerformanceReview>> GetAll();

        Task<List<PerformanceReview>> GetByEmployeeId(int employeeId);

        Task<PerformanceReview?> GetById(int id);

        Task<PerformanceReview> Create(PerformanceReview review);

        Task<PerformanceReview> Update(PerformanceReview review);

        Task<bool> Delete(int id);
    }
}