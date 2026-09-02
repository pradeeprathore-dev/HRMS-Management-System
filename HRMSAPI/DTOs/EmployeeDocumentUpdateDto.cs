using Microsoft.AspNetCore.Http;

namespace HRMSAPI.DTOs.EmployeeDocument
{
    public class EmployeeDocumentUpdateDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string DocumentType { get; set; }

        public IFormFile? File { get; set; }

        public bool IsVerified { get; set; }

        public string? Remarks { get; set; }
    }
}