using Helpers;
using HRMSAPI.DTOs;
using HRMSAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMSAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        // ✅ GET ALL
        [Authorize(Roles = "1,2")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDto paginationDto)
        {
            var data = await _service.GetAll(paginationDto);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employees fetched successfully",
                Data = data
            });
        }
        // ✅ EMPLOYEE DROPDOWN
        [Authorize(Roles = "1,2")]
        [HttpGet("Dropdown")]
        public async Task<IActionResult> GetDropdown()
        {
            var data = await _service.GetAllForDropdown();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employees fetched successfully",
                Data = data
            });
        }

        // ✅ GET BY ID
        [Authorize(Roles = "1,2")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var emp = await _service.GetById(id);

            if (emp == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Employee not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employee fetched successfully",
                Data = emp
            });
        }

        // ✅ CREATE
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            var result = await _service.Create(dto);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employee Created Successfully",
                Data = result
            });
        }

        // ✅ UPDATE
        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateEmployeeDto dto)
        {
            var updated = await _service.Update(id, dto);

            if (!updated)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Employee not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employee Updated Successfully",
                Data = null
            });
        }

        // ✅ DELETE
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
                    Message = "Employee not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employee Deleted Successfully",
                Data = null
            });
        }
    }
}