using Entites.Model;
using Helpers;
using HRMSAPI.DTOs;

namespace HRMSAPI.Repository.Interfaces
{
    public interface IShiftRepository
    {
        Task<List<Shift>> GetAll(PaginationDto paginationDto);

        Task<Shift?> GetById(int id);

        Task<Shift> Create(Shift shift);

        Task<bool> Update(Shift shift);

        Task<bool> Delete(int id);
        Task<bool> IsShiftNameExists(string shiftName);

        Task<bool> IsShiftNameExists(string shiftName, int shiftId);

        Task<bool> HasEmployees(int shiftId);
        Task<List<Shift>> GetAllForDropdown();
    }
}