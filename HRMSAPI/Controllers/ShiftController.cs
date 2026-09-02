using Helpers;
using HRMSAPI.DTOs;
using HRMSAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMSAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _service;

        public ShiftController(IShiftService service)
        {
            _service = service;
        }

        // =========================
        // GET ALL
        // =========================

        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDto paginationDto)
        {
            var data = await _service.GetAll(paginationDto);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Shifts fetched successfully",
                Data = data
            });
        }

        // =========================
        // GET BY ID
        // =========================

        [Authorize(Roles = "1")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shift = await _service.GetById(id);

            if (shift == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Shift not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Shift fetched successfully",
                Data = shift
            });
        }
        // =========================
        // SHIFT DROPDOWN
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpGet("Dropdown")]
        public async Task<IActionResult> GetDropdown()
        {
            var data = await _service.GetAllForDropdown();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Shifts fetched successfully",
                Data = data
            });
        }

        // =========================
        // CREATE
        // =========================

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateShiftDto dto)
        {
            var result = await _service.Create(dto);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Shift created successfully",
                Data = result
            });
        }

        // =========================
        // UPDATE
        // =========================

        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateShiftDto dto)
        {
            var updated = await _service.Update(id, dto);

            if (!updated)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Shift not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Shift updated successfully",
                Data = null
            });
        }

        // =========================
        // DELETE
        // =========================

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
                    Message = "Shift not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Shift deleted successfully",
                Data = null
            });
        }
    }
}