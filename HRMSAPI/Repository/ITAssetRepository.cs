using Entites.Model;
using Helpers;
using HRMSAPI.Data;
using HRMSAPI.DTOs;
using HRMSAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Repository
{
    public class ITAssetRepository : IITAssetRepository
    {
        private readonly HRMSDbContext _context;

        public ITAssetRepository(HRMSDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL
        // =========================

        public async Task<List<ITAsset>> GetAll(PaginationDto paginationDto)
        {
            var query = _context.ITAssets
                .Include(x => x.Employee)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(paginationDto.SearchText))
            {
                string search = paginationDto.SearchText.ToLower();

                query = query.Where(x =>
                    x.AssetName.ToLower().Contains(search) ||
                    x.AssetType.ToLower().Contains(search) ||
                    x.AssetCode.ToLower().Contains(search) ||
                    x.Brand.ToLower().Contains(search) ||
                    x.Model.ToLower().Contains(search) ||
                    x.SerialNumber.ToLower().Contains(search) ||
                    x.Status.ToLower().Contains(search) ||
                    (x.Employee.FirstName + " " + x.Employee.LastName)
    .ToLower()
    .Contains(search));
            }

            query = query.OrderByDescending(x => x.Id);

            return await query
                .Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
                .Take(paginationDto.PageSize)
                .ToListAsync();
        }

        // =========================
        // GET BY ID
        // =========================

        public async Task<ITAsset?> GetById(int id)
        {
            return await _context.ITAssets
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // =========================
        // CREATE
        // =========================

        public async Task<ITAsset> Create(ITAsset asset)
        {
            await _context.ITAssets.AddAsync(asset);

            await _context.SaveChangesAsync();

            return asset;
        }

        // =========================
        // UPDATE
        // =========================

        public async Task<bool> Update(ITAsset asset)
        {
            var existing = await _context.ITAssets
                .FirstOrDefaultAsync(x => x.Id == asset.Id);

            if (existing == null)
                return false;

            existing.AssetName = asset.AssetName;
            existing.AssetType = asset.AssetType;
            existing.AssetCode = asset.AssetCode;
            existing.Brand = asset.Brand;
            existing.Model = asset.Model;
            existing.SerialNumber = asset.SerialNumber;
            existing.PurchaseDate = asset.PurchaseDate;
            existing.PurchasePrice = asset.PurchasePrice;
            existing.Status = asset.Status;
            existing.Remarks = asset.Remarks;
            existing.EmployeeId = asset.EmployeeId;

            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // DELETE
        // =========================

        public async Task<bool> Delete(int id)
        {
            var asset = await _context.ITAssets
                .FirstOrDefaultAsync(x => x.Id == id);

            if (asset == null)
                return false;

            _context.ITAssets.Remove(asset);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<ITAsset>> GetMyAssets(int employeeId, PaginationDto paginationDto)
        {
            var query = _context.ITAssets
                .Include(x => x.Employee)
                .Where(x => x.EmployeeId == employeeId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(paginationDto.SearchText))
            {
                string search = paginationDto.SearchText.ToLower();

                query = query.Where(x =>
                    x.AssetName.ToLower().Contains(search) ||
                    x.AssetType.ToLower().Contains(search) ||
                    x.AssetCode.ToLower().Contains(search) ||
                    x.Brand.ToLower().Contains(search) ||
                    x.Model.ToLower().Contains(search) ||
                    x.SerialNumber.ToLower().Contains(search) ||
                    x.Status.ToLower().Contains(search));
            }

            return await query
                .OrderByDescending(x => x.Id)
                .Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
                .Take(paginationDto.PageSize)
                .ToListAsync();
        }
    }
}