using Entites.Model;
using HRMSAPI.Data;
using HRMSAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Repository
{
    public class PerformanceReviewRepository : IPerformanceReviewRepository
    {
        private readonly HRMSDbContext _context;

        public PerformanceReviewRepository(
            HRMSDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL
        // =========================

        public async Task<List<PerformanceReview>> GetAll()
        {
            return await _context.PerformanceReviews
                .Include(x => x.Employee)
                .OrderByDescending(x => x.ReviewDate)
                .ToListAsync();
        }

        // =========================
        // GET BY EMPLOYEE
        // =========================

        public async Task<List<PerformanceReview>> GetByEmployeeId(
            int employeeId)
        {
            return await _context.PerformanceReviews
                .Include(x => x.Employee)
                .Where(x => x.EmployeeId == employeeId)
                .OrderByDescending(x => x.ReviewDate)
                .ToListAsync();
        }

        // =========================
        // GET BY ID
        // =========================

        public async Task<PerformanceReview?> GetById(
            int id)
        {
            return await _context.PerformanceReviews
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // =========================
        // CREATE
        // =========================

        public async Task<PerformanceReview> Create(
            PerformanceReview review)
        {
            _context.PerformanceReviews.Add(review);

            await _context.SaveChangesAsync();

            return review;
        }

        // =========================
        // UPDATE
        // =========================

        public async Task<PerformanceReview> Update(
            PerformanceReview review)
        {
            _context.PerformanceReviews.Update(review);

            await _context.SaveChangesAsync();

            return review;
        }

        // =========================
        // DELETE
        // =========================

        public async Task<bool> Delete(int id)
        {
            var review =
                await _context.PerformanceReviews.FindAsync(id);

            if (review == null)
                return false;

            _context.PerformanceReviews.Remove(review);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}