using Entites.Model;
using HRMSAPI.Data;
using HRMSAPI.DTOs;
using HRMSAPI.Repository.Interfaces;
using HRMSAPI.Services.Interfaces;

namespace HRMSAPI.Services
{
    public class PerformanceReviewService
        : IPerformanceReviewService
    {
        private readonly IPerformanceReviewRepository _repository;
        private readonly INotificationService _notificationService;

        private readonly HRMSDbContext _context;

        public PerformanceReviewService(
     IPerformanceReviewRepository repository,
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

        public async Task<List<PerformanceReviewResponseDto>> GetAll()
        {
            var data = await _repository.GetAll();

            return data.Select(x =>
                new PerformanceReviewResponseDto
                {
                    Id = x.Id,

                    EmployeeName =
                        x.Employee.FirstName + " " +
                        x.Employee.LastName,

                    Rating = x.Rating,

                    Reviewer = x.Reviewer,

                    Comments = x.Comments,

                    ReviewDate = x.ReviewDate,

                    PromotionRecommended =
                        x.PromotionRecommended
                }).ToList();
        }

        // =========================
        // GET BY EMPLOYEE
        // =========================

        public async Task<List<PerformanceReviewResponseDto>>
            GetByEmployeeId(int employeeId)
        {
            var data =
                await _repository.GetByEmployeeId(employeeId);

            return data.Select(x =>
                new PerformanceReviewResponseDto
                {
                    Id = x.Id,

                    EmployeeName =
                        x.Employee.FirstName + " " +
                        x.Employee.LastName,

                    Rating = x.Rating,

                    Reviewer = x.Reviewer,

                    Comments = x.Comments,

                    ReviewDate = x.ReviewDate,

                    PromotionRecommended =
                        x.PromotionRecommended
                }).ToList();
        }

        // =========================
        // GET BY ID
        // =========================

        public async Task<PerformanceReviewResponseDto?> GetById(
            int id)
        {
            var x = await _repository.GetById(id);

            if (x == null)
                return null;

            return new PerformanceReviewResponseDto
            {
                Id = x.Id,

                EmployeeName =
                    x.Employee.FirstName + " " +
                    x.Employee.LastName,

                Rating = x.Rating,

                Reviewer = x.Reviewer,

                Comments = x.Comments,

                ReviewDate = x.ReviewDate,

                PromotionRecommended =
                    x.PromotionRecommended
            };
        }

        // =========================
        // CREATE
        // =========================

        public async Task<PerformanceReviewResponseDto> Create(
            CreatePerformanceReviewDto dto)
        {
            var employee =
                await _context.Employees.FindAsync(dto.EmployeeId);

            if (employee == null)
                throw new Exception("Employee not found");

            if (dto.Rating < 1 || dto.Rating > 5)
                throw new Exception("Rating must be between 1 and 5");

            var review =
                new PerformanceReview
                {
                    EmployeeId = dto.EmployeeId,

                    Rating = dto.Rating,

                    Reviewer = dto.Reviewer,

                    Comments = dto.Comments,

                    ReviewDate = dto.ReviewDate,

                    PromotionRecommended =
                        dto.PromotionRecommended
                };

            await _repository.Create(review);
            await _notificationService.Create(
    new CreateNotificationDto
    {
        EmployeeId = review.EmployeeId,

        Title = "Performance Review Completed",

        Message =
            $"Rating: {review.Rating}/5 | Promotion Recommended: {(review.PromotionRecommended ? "Yes" : "No")}"
    });

            return new PerformanceReviewResponseDto
            {
                Id = review.Id,

                EmployeeName =
                    employee.FirstName + " " +
                    employee.LastName,

                Rating = review.Rating,

                Reviewer = review.Reviewer,

                Comments = review.Comments,

                ReviewDate = review.ReviewDate,

                PromotionRecommended =
                    review.PromotionRecommended
            };
        }

        // =========================
        // UPDATE
        // =========================

        public async Task<bool> Update(
            int id,
            UpdatePerformanceReviewDto dto)
        {
            var review =
                await _repository.GetById(id);

            if (review == null)
                return false;

            if (dto.Rating < 1 || dto.Rating > 5)
                throw new Exception("Rating must be between 1 and 5");

            review.Rating = dto.Rating;
            review.Reviewer = dto.Reviewer;
            review.Comments = dto.Comments;
            review.ReviewDate = dto.ReviewDate;
            review.PromotionRecommended =
                dto.PromotionRecommended;

            await _repository.Update(review);

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