using Entites.Model;
using Helpers;
using HRMSAPI.DTOs;

namespace HRMSAPI.Services.Interfaces
{
    public interface IITAssetService
    {
        Task<List<ITAssetResponseDto>> GetAll(PaginationDto paginationDto);

        Task<ITAssetResponseDto?> GetById(int id);

        Task<ITAssetResponseDto> Create(CreateITAssetDto dto);

        Task<bool> Update(int id, UpdateITAssetDto dto);

        Task<bool> Delete(int id);
        Task<List<ITAssetResponseDto>> GetMyAssets(int employeeId, PaginationDto paginationDto);
    }
}