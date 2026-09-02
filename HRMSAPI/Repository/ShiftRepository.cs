using Entites.Model;
using Helpers;
using HRMSAPI.Data;
using HRMSAPI.DTOs;
using HRMSAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Repository
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly HRMSDbContext _context;

        public ShiftRepository(HRMSDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL
        // =========================

        public async Task<List<Shift>> GetAll(PaginationDto paginationDto)
        {
            var query = _context.Shifts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(paginationDto.SearchText))
            {
                string search = paginationDto.SearchText.ToLower();

                query = query.Where(x =>
                    x.ShiftName.ToLower().Contains(search));
            }

            query = query.OrderBy(x => x.StartTime);

            return await query
                .Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
                .Take(paginationDto.PageSize)
                .ToListAsync();
        }

        // =========================
        // GET BY ID
        // =========================

        public async Task<Shift?> GetById(int id)
        {
            return await _context.Shifts
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // =========================
        // CREATE
        // =========================

        public async Task<Shift> Create(Shift shift)
        {
            await _context.Shifts.AddAsync(shift);

            await _context.SaveChangesAsync();

            return shift;
        }

        // =========================
        // UPDATE
        // =========================

        public async Task<bool> Update(Shift shift)
        {
            var existing = await _context.Shifts
                .FirstOrDefaultAsync(x => x.Id == shift.Id);

            if (existing == null)
                return false;

            existing.ShiftName = shift.ShiftName;
            existing.StartTime = shift.StartTime;
            existing.EndTime = shift.EndTime;
            existing.GraceMinutes = shift.GraceMinutes;
            existing.HalfDayTime = shift.HalfDayTime;
            existing.IsActive = shift.IsActive;

            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // DELETE
        // =========================

        public async Task<bool> Delete(int id)
        {
            var shift = await _context.Shifts
                .FirstOrDefaultAsync(x => x.Id == id);

            if (shift == null)
                return false;

            _context.Shifts.Remove(shift);

            await _context.SaveChangesAsync();

            return true;
        }
        // =========================
        // SHIFT NAME EXISTS
        // =========================

        public async Task<bool> IsShiftNameExists(string shiftName)
        {
            return await _context.Shifts
                .AnyAsync(x => x.ShiftName.ToLower() == shiftName.ToLower());
        }

        // =========================
        // SHIFT NAME EXISTS (UPDATE)
        // =========================

        public async Task<bool> IsShiftNameExists(string shiftName, int shiftId)
        {
            return await _context.Shifts
                .AnyAsync(x =>
                    x.Id != shiftId &&
                    x.ShiftName.ToLower() == shiftName.ToLower());
        }

        // =========================
        // HAS EMPLOYEES
        // =========================

        public async Task<bool> HasEmployees(int shiftId)
        {
            return await _context.Employees
                .AnyAsync(x => x.ShiftId == shiftId);
        }
        // =========================
        // DROPDOWN
        // =========================

        public async Task<List<Shift>> GetAllForDropdown()
        {
            return await _context.Shifts
                .Where(x => x.IsActive)
                .OrderBy(x => x.ShiftName)
                .ToListAsync();
        }
    }
}