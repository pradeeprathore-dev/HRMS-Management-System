using Entites.Model;
using HRMSAPI.Data;
using HRMSAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Repository
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly HRMSDbContext _context;

        public HolidayRepository(HRMSDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL
        // =========================

        public async Task<List<Holiday>> GetAll()
        {
            return await _context.Holidays
                .OrderBy(x => x.Date)
                .ToListAsync();
        }

        // =========================
        // GET BY ID
        // =========================

        public async Task<Holiday?> GetById(int id)
        {
            return await _context.Holidays
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // =========================
        // CREATE
        // =========================

        public async Task<Holiday> Create(Holiday holiday)
        {
            _context.Holidays.Add(holiday);

            await _context.SaveChangesAsync();

            return holiday;
        }

        // =========================
        // UPDATE
        // =========================

        public async Task<Holiday> Update(Holiday holiday)
        {
            _context.Holidays.Update(holiday);

            await _context.SaveChangesAsync();

            return holiday;
        }

        // =========================
        // DELETE
        // =========================

        public async Task<bool> Delete(int id)
        {
            var holiday =
                await _context.Holidays.FindAsync(id);

            if (holiday == null)
                return false;

            _context.Holidays.Remove(holiday);

            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // DATE EXISTS
        // =========================

        public async Task<bool> Exists(DateTime date)
        {
            return await _context.Holidays
                .AnyAsync(x => x.Date.Date == date.Date);
        }
    }
}