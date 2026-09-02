using HRMSAPI.DTOs;

namespace HRMSAPI.Services.Interfaces
{
    public interface IHolidayService
    {
        Task<List<HolidayResponseDto>> GetAll();

        Task<HolidayResponseDto?> GetById(int id);

        Task<HolidayResponseDto> Create(CreateHolidayDto dto);

        Task<bool> Update(int id, CreateHolidayDto dto);

        Task<bool> Delete(int id);
    }
}