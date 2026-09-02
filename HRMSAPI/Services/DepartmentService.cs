using Entites.Model;
using HRMSAPI.DTOs;

namespace HRMSAPI.Services
{
    public interface DepartmentService
    {
        Task<List<Department>> GetAll();

        Task<Department> GetById(int id);

        Task<Department> Create(
            CreateDepartmentDto dto);

        Task<bool> Update(
            int id,
            UpdateDepartmentDto dto);

        Task<bool> Delete(int id);
    }
}
