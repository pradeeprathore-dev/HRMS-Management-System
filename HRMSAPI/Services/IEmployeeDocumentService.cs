using HRMSAPI.DTOs.EmployeeDocument;

namespace HRMSAPI.Services.Interfaces
{
    public interface IEmployeeDocumentService
    {
        Task<List<EmployeeDocumentResponseDto>> GetAll();

        Task<EmployeeDocumentResponseDto?> GetById(int id);
        Task<string?> GetFilePath(int id);

        Task<EmployeeDocumentResponseDto> Create(EmployeeDocumentCreateDto dto);

        Task<bool> Update(int id, EmployeeDocumentUpdateDto dto);

        Task<bool> Delete(int id);


    }
}