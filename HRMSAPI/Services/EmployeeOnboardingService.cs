using Entites.Model;
using HRMSAPI.DTOs.EmployeeOnboarding;
using HRMSAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Services
{
    public class EmployeeOnboardingService : IEmployeeOnboardingService
    {
        private readonly IEmployeeOnboardingRepository _repository;

        public EmployeeOnboardingService(
            IEmployeeOnboardingRepository repository)
        {
            _repository = repository;
        }

        // ===========================
        // GET ALL
        // ===========================

        public async Task<List<EmployeeOnboardingResponseDto>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();

            return data.Select(x => new EmployeeOnboardingResponseDto
            {
                Id = x.Id,

                EmployeeId = x.EmployeeId,

                EmployeeName =
                    x.Employee.FirstName + " " + x.Employee.LastName,

                JoiningDate = x.JoiningDate,

                OfficialEmail = x.OfficialEmail,

                SeatNumber = x.SeatNumber,

                CredentialsCreated = x.CredentialsCreated,

                DocumentsVerified = x.DocumentsVerified,

                LaptopAllocated = x.LaptopAllocated,

                ManagerAssigned = x.ManagerAssigned,

                ProjectAssigned = x.ProjectAssigned,

                IdCardGenerated = x.IdCardGenerated,

                IsCompleted = x.IsCompleted,

                Progress = CalculateProgress(x)

            }).ToList();
        }

        // ===========================
        // GET BY ID
        // ===========================

        public async Task<EmployeeOnboardingResponseDto?> GetByIdAsync(int id)
        {
            var x = await _repository.GetByIdAsync(id);

            if (x == null)
                return null;

            return new EmployeeOnboardingResponseDto
            {
                Id = x.Id,

                EmployeeId = x.EmployeeId,

                EmployeeName =
                    x.Employee.FirstName + " " + x.Employee.LastName,

                JoiningDate = x.JoiningDate,

                OfficialEmail = x.OfficialEmail,

                SeatNumber = x.SeatNumber,

                CredentialsCreated = x.CredentialsCreated,

                DocumentsVerified = x.DocumentsVerified,

                LaptopAllocated = x.LaptopAllocated,

                ManagerAssigned = x.ManagerAssigned,

                ProjectAssigned = x.ProjectAssigned,

                IdCardGenerated = x.IdCardGenerated,

                IsCompleted = x.IsCompleted,

                Progress = CalculateProgress(x)
            };
        }

        // ===========================
        // CREATE
        // ===========================

        public async Task<EmployeeOnboardingResponseDto> CreateAsync(
            EmployeeOnboardingCreateDto dto)
        {
            if (!await _repository.EmployeeExistsAsync(dto.EmployeeId))
            {
                throw new Exception("Employee not found.");
            }

            var alreadyExists =
                await _repository.GetByEmployeeIdAsync(dto.EmployeeId);

            if (alreadyExists != null)
            {
                throw new Exception(
                    "Onboarding already exists for this employee.");
            }

            var onboarding = new EmployeeOnboarding
            {
                EmployeeId = dto.EmployeeId,

                JoiningDate = dto.JoiningDate,

                OfficialEmail = dto.OfficialEmail,

                SeatNumber = dto.SeatNumber,

                CredentialsCreated =
                    await _repository.UserExistsAsync(dto.EmployeeId),

                DocumentsVerified =
                    await _repository.DocumentsUploadedAsync(dto.EmployeeId),

                LaptopAllocated =
                    await _repository.LaptopAllocatedAsync(dto.EmployeeId),

                ProjectAssigned =
                    await _repository.ProjectAssignedAsync(dto.EmployeeId),

                ManagerAssigned = dto.ManagerAssigned,

                IdCardGenerated = dto.IdCardGenerated,

                IsCompleted = dto.IsCompleted
            };

            var result =
                await _repository.CreateAsync(onboarding);

            var saved =
                await _repository.GetByIdAsync(result.Id);

            return new EmployeeOnboardingResponseDto
            {
                Id = saved.Id,

                EmployeeId = saved.EmployeeId,

                EmployeeName =
                    saved.Employee.FirstName + " " + saved.Employee.LastName,

                JoiningDate = saved.JoiningDate,

                OfficialEmail = saved.OfficialEmail,

                SeatNumber = saved.SeatNumber,

                CredentialsCreated = saved.CredentialsCreated,

                DocumentsVerified = saved.DocumentsVerified,

                LaptopAllocated = saved.LaptopAllocated,

                ManagerAssigned = saved.ManagerAssigned,

                ProjectAssigned = saved.ProjectAssigned,

                IdCardGenerated = saved.IdCardGenerated,

                IsCompleted = saved.IsCompleted,

                Progress = CalculateProgress(saved)
            };
        }

        // ===========================
        // UPDATE
        // ===========================

        public async Task<EmployeeOnboardingResponseDto?> UpdateAsync(
            EmployeeOnboardingUpdateDto dto)
        {
            var existing =
                await _repository.GetByIdAsync(dto.Id);

            if (existing == null)
                return null;

            existing.JoiningDate = dto.JoiningDate;

            existing.OfficialEmail = dto.OfficialEmail;

            existing.SeatNumber = dto.SeatNumber;

            existing.ManagerAssigned = dto.ManagerAssigned;

            existing.IdCardGenerated = dto.IdCardGenerated;

            existing.IsCompleted = dto.IsCompleted;

            existing.CredentialsCreated =
                await _repository.UserExistsAsync(existing.EmployeeId);

            existing.DocumentsVerified =
                await _repository.DocumentsUploadedAsync(existing.EmployeeId);

            existing.LaptopAllocated =
                await _repository.LaptopAllocatedAsync(existing.EmployeeId);

            existing.ProjectAssigned =
                await _repository.ProjectAssignedAsync(existing.EmployeeId);

            var updated =
                await _repository.UpdateAsync(existing);

            if (updated == null)
                return null;

            var response =
                await _repository.GetByIdAsync(updated.Id);

            return new EmployeeOnboardingResponseDto
            {
                Id = response.Id,

                EmployeeId = response.EmployeeId,

                EmployeeName =
                    response.Employee.FirstName + " " + response.Employee.LastName,

                JoiningDate = response.JoiningDate,

                OfficialEmail = response.OfficialEmail,

                SeatNumber = response.SeatNumber,

                CredentialsCreated = response.CredentialsCreated,

                DocumentsVerified = response.DocumentsVerified,

                LaptopAllocated = response.LaptopAllocated,

                ManagerAssigned = response.ManagerAssigned,

                ProjectAssigned = response.ProjectAssigned,

                IdCardGenerated = response.IdCardGenerated,

                IsCompleted = response.IsCompleted,

                Progress = CalculateProgress(response)
            };
        }

        // ===========================
        // DELETE
        // ===========================

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        // ===========================
        // PRIVATE HELPER
        // ===========================

        private int CalculateProgress(EmployeeOnboarding onboarding)
        {
            int total = 6;
            int completed = 0;

            if (onboarding.CredentialsCreated)
                completed++;

            if (onboarding.DocumentsVerified)
                completed++;

            if (onboarding.LaptopAllocated)
                completed++;

            if (onboarding.ManagerAssigned)
                completed++;

            if (onboarding.ProjectAssigned)
                completed++;

            if (onboarding.IdCardGenerated)
                completed++;

            return (completed * 100) / total;
        }
    }
}