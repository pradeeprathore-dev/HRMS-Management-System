using Helpers;
using HRMSAPI.DTOs.EmployeeOnboarding;
using HRMSAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMSAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeOnboardingController : ControllerBase
    {
        private readonly IEmployeeOnboardingService _service;

        public EmployeeOnboardingController(
            IEmployeeOnboardingService service)
        {
            _service = service;
        }

        // =========================
        // GET ALL
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employee Onboarding fetched successfully.",
                Data = data
            });
        }

        // =========================
        // GET BY ID
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Onboarding record not found.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employee Onboarding fetched successfully.",
                Data = data
            });
        }

        // =========================
        // CREATE
        // =========================

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> Create(
            EmployeeOnboardingCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employee Onboarding created successfully.",
                Data = result
            });
        }

        // =========================
        // UPDATE
        // =========================

        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            EmployeeOnboardingUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid Id.",
                    Data = null
                });
            }

            var result =
                await _service.UpdateAsync(dto);

            if (result == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Onboarding record not found.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employee Onboarding updated successfully.",
                Data = result
            });
        }

        // =========================
        // DELETE
        // =========================

        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _service.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Onboarding record not found.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Employee Onboarding deleted successfully.",
                Data = null
            });
        }
    }
}