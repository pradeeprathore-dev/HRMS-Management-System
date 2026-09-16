using HRMSAPI.DTOs;

namespace HRMSAPI.Services
{
    public interface IEmployeeService
    {
        //Task<List<EmployeeResponseDto>> GetAll(PaginationDto paginationDto);
        Task<PaginationResponseDto<EmployeeResponseDto>> GetAll(
    PaginationDto paginationDto);

        Task<EmployeeResponseDto?> GetById(int id);

        Task<EmployeeResponseDto> Create(CreateEmployeeDto dto);

        Task<bool> Update(int id, UpdateEmployeeDto dto);

        Task<bool> Delete(int id);
        Task<List<EmployeeDropdownDto>> GetAllForDropdown();

    }
}