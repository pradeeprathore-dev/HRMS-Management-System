using Entites.Model;
using HRMSAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Repository
{
    public class EmployeeOnboardingRepository
        : IEmployeeOnboardingRepository
    {
        private readonly HRMSDbContext _context;

        public EmployeeOnboardingRepository(
            HRMSDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeOnboarding>> GetAllAsync()
        {
            return await _context.EmployeeOnboardings
                .Include(x => x.Employee)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<EmployeeOnboarding?> GetByIdAsync(int id)
        {
            return await _context.EmployeeOnboardings
                .Include(x => x.Employee)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<EmployeeOnboarding> CreateAsync(EmployeeOnboarding onboarding)
        {
            _context.EmployeeOnboardings.Add(onboarding);

            await _context.SaveChangesAsync();

            return onboarding;
        }

        public async Task<EmployeeOnboarding?> UpdateAsync(EmployeeOnboarding onboarding)
        {
            var existing =
                await _context.EmployeeOnboardings
                .FirstOrDefaultAsync(x => x.Id == onboarding.Id);

            if (existing == null)
                return null;

            existing.JoiningDate = onboarding.JoiningDate;
            existing.OfficialEmail = onboarding.OfficialEmail;
            existing.SeatNumber = onboarding.SeatNumber;
            existing.CredentialsCreated = onboarding.CredentialsCreated;
            existing.DocumentsVerified = onboarding.DocumentsVerified;
            existing.LaptopAllocated = onboarding.LaptopAllocated;
            existing.ManagerAssigned = onboarding.ManagerAssigned;
            existing.ProjectAssigned = onboarding.ProjectAssigned;
            existing.IdCardGenerated = onboarding.IdCardGenerated;
            existing.IsCompleted = onboarding.IsCompleted;

            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing =
                await _context.EmployeeOnboardings
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existing == null)
                return false;

            _context.EmployeeOnboardings.Remove(existing);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> EmployeeExistsAsync(int employeeId)
        {
            return await _context.Employees
                .AnyAsync(x => x.Id == employeeId);
        }

        public async Task<bool> UserExistsAsync(int employeeId)
        {
            return await _context.Users
                .AnyAsync(x => x.EmployeeId == employeeId);
        }

        public async Task<bool> DocumentsUploadedAsync(int employeeId)
        {
            return await _context.Documents
                .AnyAsync(x => x.EmployeeId == employeeId);
        }

        public async Task<bool> LaptopAllocatedAsync(int employeeId)
        {
            return await _context.ITAssets
                .AnyAsync(x => x.EmployeeId == employeeId);
        }

        public async Task<bool> ProjectAssignedAsync(int employeeId)
        {
            return await _context.EmployeeProjects
                .AnyAsync(x => x.EmployeeId == employeeId);
        }

        public async Task<EmployeeOnboarding?> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.EmployeeOnboardings
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
        }
    }
}