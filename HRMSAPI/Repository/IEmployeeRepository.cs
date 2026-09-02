using Entites.Model;
using HRMSAPI.DTOs;

namespace HRMSAPI.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAll(PaginationDto paginationDto);

        Task<Employee?> GetById(int id);

        Task<Employee> Create(Employee employee);

        Task<Employee> Update(Employee employee);

        Task<bool> Delete(int id);

        Task<Employee?> GetByEmail(string email);
        Task<List<Employee>> GetAllForDropdown();
    }
}