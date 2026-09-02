using Entites.Model;
using HRMSAPI.DTOs.EmployeeDocument;

namespace HRMSAPI.Repository.Interfaces
{
    public interface IEmployeeDocumentRepository
    {
        Task<List<EmployeeDocument>> GetAll();

        Task<EmployeeDocument?> GetById(int id);
        Task<string?> GetFilePath(int id);

        Task<EmployeeDocument> Create(EmployeeDocument employeeDocument);

        Task<EmployeeDocument> Update(EmployeeDocument employeeDocument);

        Task<bool> Delete(int id);

    }
}