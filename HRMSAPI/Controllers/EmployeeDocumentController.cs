using Helpers;
using HRMSAPI.DTOs.EmployeeDocument;
using HRMSAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HRMSAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDocumentController : ControllerBase
    {
        private readonly IEmployeeDocumentService _service;

        public EmployeeDocumentController(IEmployeeDocumentService service)
        {
            _service = service;
        }

        // GET ALL
        [Authorize(Roles = "1,2")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employee Documents fetched successfully",
                Data = data
            });
        }

        // GET BY ID
        [Authorize(Roles = "1,2")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var document = await _service.GetById(id);

            if (document == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Document not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employee Document fetched successfully",
                Data = document
            });
        }

        [Authorize(Roles = "1,2")]
        [HttpGet("preview/{id}")]
        public async Task<IActionResult> Preview(int id)
        {
            var filePath = await _service.GetFilePath(id);

            if (string.IsNullOrEmpty(filePath))
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Document not found.",
                    Data = null
                });
            }

            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                filePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Physical file not found.",
                    Data = null
                });
            }

            var contentType = "application/octet-stream";

            var extension = Path.GetExtension(fullPath).ToLower();

            switch (extension)
            {
                case ".pdf":
                    contentType = "application/pdf";
                    break;

                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;

                case ".png":
                    contentType = "image/png";
                    break;

                case ".doc":
                    contentType = "application/msword";
                    break;

                case ".docx":
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(fullPath);

            return File(bytes, contentType);
        }

        // CREATE
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EmployeeDocumentCreateDto dto)
        {
            var result = await _service.Create(dto);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employee Document uploaded successfully",
                Data = result
            });
        }

        // UPDATE
        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] EmployeeDocumentUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid Document Id",
                    Data = null
                });
            }
            var updated = await _service.Update(id, dto);
            if (!updated)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Document not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employee Document updated successfully",
                Data = updated
            });
        }

        // DELETE
        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.Delete(id);

            if (!deleted)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Document not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employee Document deleted successfully",
                Data = null
            });
        }
    }
}