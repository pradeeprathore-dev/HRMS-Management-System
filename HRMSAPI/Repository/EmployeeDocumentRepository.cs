using Entites.Model;
using HRMSAPI.Data;
using HRMSAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Repository
{
    public class EmployeeDocumentRepository : IEmployeeDocumentRepository
    {
        private readonly HRMSDbContext _context;

        public EmployeeDocumentRepository(HRMSDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL
        // =========================

        public async Task<List<EmployeeDocument>> GetAll()
        {
            return await _context.EmployeeDocuments
                .Include(x => x.Employee)
                .ToListAsync();
        }

        // =========================
        // GET BY ID
        // =========================

        public async Task<EmployeeDocument?> GetById(int id)
        {
            return await _context.EmployeeDocuments
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // =========================
        // FILE PATH
        // =========================

        public async Task<string?> GetFilePath(int id)
        {
            var document = await _context.EmployeeDocuments.FindAsync(id);
            return document?.FilePath;
        }

        // =========================
        // CREATE
        // =========================

        public async Task<EmployeeDocument> Create(EmployeeDocument employeeDocument)
        {
            _context.EmployeeDocuments.Add(employeeDocument);

            await _context.SaveChangesAsync();

            return employeeDocument;
        }

        // =========================
        // UPDATE
        // =========================

        public async Task<EmployeeDocument> Update(EmployeeDocument employeeDocument)
        {
            _context.EmployeeDocuments.Update(employeeDocument);

            await _context.SaveChangesAsync();

            return employeeDocument;
        }

        // =========================
        // DELETE
        // =========================

        public async Task<bool> Delete(int id)
        {
            var document = await _context.EmployeeDocuments.FindAsync(id);

            if (document == null)
                return false;

            _context.EmployeeDocuments.Remove(document);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}