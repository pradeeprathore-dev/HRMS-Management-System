using Helpers;
using HRMSAPI.DTOs;

namespace HRMSAPI.Services.Interfaces
{
    public interface IShiftService
    {
        Task<List<ShiftResponseDto>> GetAll(PaginationDto paginationDto);

        Task<ShiftResponseDto?> GetById(int id);

        Task<ShiftResponseDto> Create(CreateShiftDto dto);

        Task<bool> Update(int id, UpdateShiftDto dto);

        Task<bool> Delete(int id);
        Task<List<ShiftDropdownDto>> GetAllForDropdown();
    }
}