using Entites.Model;

namespace HRMSAPI.Repository
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAll();

        Task<Department> GetById(int id);

        Task<Department> Create(Department department);

        Task<Department> Update(Department department);

        Task<bool> Delete(int id);
    }
}
