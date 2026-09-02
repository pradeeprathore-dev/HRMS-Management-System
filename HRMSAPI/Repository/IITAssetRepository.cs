using Entites.Model;
using Helpers;
using HRMSAPI.DTOs;

namespace HRMSAPI.Repository.Interfaces
{
    public interface IITAssetRepository
    {
        Task<List<ITAsset>> GetAll(PaginationDto paginationDto);
        Task<List<ITAsset>> GetMyAssets(int employeeId, PaginationDto paginationDto);
        Task<ITAsset?> GetById(int id);

        Task<ITAsset> Create(ITAsset asset);

        Task<bool> Update(ITAsset asset);

        Task<bool> Delete(int id);
        
    }
}