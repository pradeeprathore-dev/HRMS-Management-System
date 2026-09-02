using Entites.Model;
using HRMSAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HRMSDbContext _context;

        public DepartmentRepository(
            HRMSDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetAll()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department> GetById(int id)
        {
            return await _context.Departments
                .FindAsync(id);
        }

        public async Task<Department> Create(
            Department department)
        {
            _context.Departments.Add(department);

            await _context.SaveChangesAsync();

            return department;
        }

        public async Task<Department> Update(
            Department department)
        {
            _context.Departments.Update(department);

            await _context.SaveChangesAsync();

            return department;
        }

        public async Task<bool> Delete(int id)
        {
            var department =
                await _context.Departments.FindAsync(id);

            if (department == null)
                return false;

            bool hasEmployees =
                await _context.Employees
                    .AnyAsync(x => x.DepartmentId == id);

            if (hasEmployees)
            {
                throw new Exception(
                    "Department is assigned to employees. Please move employees before deleting.");
            }

            _context.Departments.Remove(department);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
