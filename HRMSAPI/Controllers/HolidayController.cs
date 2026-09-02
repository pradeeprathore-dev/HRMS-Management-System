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
    public class HolidayController : ControllerBase
    {
        private readonly IHolidayService _service;

        public HolidayController(
            IHolidayService service)
        {
            _service = service;
        }

        // =========================
        // GET ALL
        // =========================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Holidays fetched successfully",
                Data = data
            });
        }

        // =========================
        // GET BY ID
        // =========================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var holiday = await _service.GetById(id);

            if (holiday == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Holiday not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Holiday fetched successfully",
                Data = holiday
            });
        }

        // =========================
        // CREATE
        // =========================

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateHolidayDto dto)
        {
            var holiday =
                await _service.Create(dto);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Holiday created successfully",
                Data = holiday
            });
        }

        // =========================
        // UPDATE
        // =========================

        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            CreateHolidayDto dto)
        {
            var updated =
                await _service.Update(id, dto);

            if (!updated)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Holiday not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Holiday updated successfully",
                Data = null
            });
        }

        // =========================
        // DELETE
        // =========================

        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id)
        {
            var deleted =
                await _service.Delete(id);

            if (!deleted)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Holiday not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Holiday deleted successfully",
                Data = null
            });
        }
    }
}