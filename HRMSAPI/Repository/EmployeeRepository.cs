using Entites.Model;
using HRMSAPI.Data;
using HRMSAPI.DTOs;
using HRMSAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRMSDbContext _context;

        public EmployeeRepository(HRMSDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL
        // =========================

        public async Task<List<Employee>> GetAll(PaginationDto paginationDto)
        {
            var query = _context.Employees
    .Include(e => e.Department)
    .Include(e => e.Designation)
    .Include(e => e.Shift)
    .AsQueryable();

            // =========================
            // SEARCH
            // =========================

            if (!string.IsNullOrEmpty(paginationDto.SearchText))
            {
                query = query.Where(e =>
                    e.FirstName.Contains(paginationDto.SearchText) ||
                    e.LastName.Contains(paginationDto.SearchText) ||
                    e.Email.Contains(paginationDto.SearchText));
            }

            // =========================
            // DEPARTMENT FILTER
            // =========================

            if (paginationDto.DepartmentId.HasValue)
            {
                query = query.Where(e =>
                    e.DepartmentId == paginationDto.DepartmentId.Value);
            }

            // =========================
            // SORTING
            // =========================

            if (!string.IsNullOrEmpty(paginationDto.SortBy))
            {
                switch (paginationDto.SortBy.ToLower())
                {
                    case "firstname":

                        query = paginationDto.SortOrder?.ToLower() == "desc"
                            ? query.OrderByDescending(e => e.FirstName)
                            : query.OrderBy(e => e.FirstName);

                        break;

                    case "salary":

                        query = paginationDto.SortOrder?.ToLower() == "desc"
                            ? query.OrderByDescending(e => e.Salary)
                            : query.OrderBy(e => e.Salary);

                        break;
                }
            }

            // =========================
            // PAGINATION
            // =========================

            return await query
                .Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
                .Take(paginationDto.PageSize)
                .ToListAsync();
        }

        // =========================
        // GET BY ID
        // =========================

        public async Task<Employee> GetById(int id)
        {
            return await _context.Employees
    .Include(e => e.Department)
    .Include(e => e.Designation)
    .Include(e => e.Shift)
    .FirstOrDefaultAsync(x => x.Id == id);
        }

        // =========================
        // CREATE
        // =========================

        public async Task<Employee> Create(Employee employee)
        {
            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return employee;
        }

        // =========================
        // UPDATE
        // =========================

        public async Task<Employee> Update(Employee employee)
        {
            _context.Employees.Update(employee);

            await _context.SaveChangesAsync();

            return employee;
        }

        // =========================
        // DELETE
        // =========================

        public async Task<bool> Delete(int id)
        {
            var emp = await _context.Employees.FindAsync(id);

            if (emp == null)
                return false;

            _context.Employees.Remove(emp);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<Employee>> GetAllForDropdown()
        {
            return await _context.Employees
                .OrderBy(e => e.FirstName)
                .ToListAsync();
        }

        // =========================
        // GET BY EMAIL
        // =========================

        public async Task<Employee?> GetByEmail(string email)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}