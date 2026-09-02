using Entites.Model;
using HRMSAPI.Data;
using HRMSAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        private readonly HRMSDbContext _context;

        public DesignationController(HRMSDbContext context)
        {
            _context = context;
        }

        // ✅ GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Designations
                .Select(d => new DesignationResponseDto
                {
                    Id = d.Id,
                    Title = d.Title
                })
                .ToListAsync();

            return Ok(data);
        }

        // ✅ GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var desig = await _context.Designations
                .Where(d => d.Id == id)
                .Select(d => new DesignationResponseDto
                {
                    Id = d.Id,
                    Title = d.Title
                })
                .FirstOrDefaultAsync();

            if (desig == null)
                return NotFound();

            return Ok(desig);
        }

        // ✅ CREATE
        [HttpPost]
        public async Task<IActionResult> Create(CreateDesignationDto dto)
        {
            var desig = new Designation
            {
                Title = dto.Title
            };
            if (await _context.Designations
    .AnyAsync(x => x.Title == dto.Title))
            {
                return BadRequest(
                    "Designation already exists");
            }

            _context.Designations.Add(desig);
            await _context.SaveChangesAsync();

            return Ok(desig);
        }

        // ✅ UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateDesignationDto dto)
        {
            var desig = await _context.Designations.FindAsync(id);

            if (desig == null)
                return NotFound();

            desig.Title = dto.Title;

            await _context.SaveChangesAsync();

            return Ok(desig);
        }

        // ✅ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var desig = await _context.Designations.FindAsync(id);

            if (desig == null)
                return NotFound();
            var hasEmployees =
    await _context.Employees
    .AnyAsync(x => x.DesignationId == id);

            if (hasEmployees)
            {
                return BadRequest(
                    "Designation is assigned to employees");
            }

            _context.Designations.Remove(desig);
            await _context.SaveChangesAsync();

            return Ok("Deleted Successfully");
        }
    }
}