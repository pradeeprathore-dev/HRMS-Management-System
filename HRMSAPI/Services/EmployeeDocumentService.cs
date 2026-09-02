using HRMSAPI.DTOs.EmployeeDocument;
using HRMSAPI.Repository.Interfaces;
using HRMSAPI.Services.Interfaces;
using AutoMapper;
using Entites.Model;
using HRMSAPI.Data;
using HRMSAPI.DTOs.EmployeeDocument;
using HRMSAPI.Repository.Interfaces;
using HRMSAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Services
{
    public class EmployeeDocumentService : IEmployeeDocumentService
    {
        private readonly IEmployeeDocumentRepository _repository;


        private readonly IMapper _mapper;

        private readonly HRMSDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public EmployeeDocumentService(
     IEmployeeDocumentRepository repository,
     IMapper mapper,
     HRMSDbContext context,
     IWebHostEnvironment environment)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
            _environment = environment;
        }

        public async Task<List<EmployeeDocumentResponseDto>> GetAll()
        {
            var documents = await _repository.GetAll();

            return _mapper.Map<List<EmployeeDocumentResponseDto>>(documents);
        }

        public async Task<EmployeeDocumentResponseDto?> GetById(int id)
        {
            var document = await _repository.GetById(id);

            if (document == null)
                return null;

            return _mapper.Map<EmployeeDocumentResponseDto>(document);
        }

        public async Task<EmployeeDocumentResponseDto> Create(EmployeeDocumentCreateDto dto)
        {
            // Employee Exists Check
            var employee = await _context.Employees
                .FirstOrDefaultAsync(x => x.Id == dto.EmployeeId);

            if (employee == null)
            {
                throw new Exception("Employee not found.");
            }

            // File Check
            if (dto.File == null || dto.File.Length == 0)
            {
                throw new Exception("Please select a file.");
            }

            // Extension Check
            string extension = Path.GetExtension(dto.File.FileName);

            if (!FileHelper.IsValidExtension(extension))
            {
                throw new Exception("Invalid file type.");
            }

            // Size Check
            if (!FileHelper.IsValidFileSize(dto.File))
            {
                throw new Exception("Maximum allowed file size is 5 MB.");
            }

            // Upload Folder
            string uploadFolder = Path.Combine(
                _environment.WebRootPath,
                "Uploads",
                "Documents");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            // Generate Unique Name
            string uniqueFileName =
                FileHelper.GenerateUniqueFileName(dto.File.FileName);

            string filePath =
                Path.Combine(uploadFolder, uniqueFileName);

            // Save Physical File
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            // Create Entity
            var document = new EmployeeDocument
            {
                EmployeeId = dto.EmployeeId,
                DocumentType = dto.DocumentType,
                FileName = dto.File.FileName,
                FilePath = "/Uploads/Documents/" + uniqueFileName,
                FileExtension = extension,
                FileSize = dto.File.Length,
                UploadedDate = DateTime.Now,
                IsVerified = false,
                Remarks = dto.Remarks
            };

            // Save DB
            await _repository.Create(document);

            // Reload With Navigation Property
            var savedDocument =
                await _repository.GetById(document.Id);

            return _mapper.Map<EmployeeDocumentResponseDto>(savedDocument);
        }
        public async Task<string?> GetFilePath(int id)
        {
            return await _repository.GetFilePath(id);
        }

        public async Task<bool> Update(int id, EmployeeDocumentUpdateDto dto)
        {
            var document = await _repository.GetById(id);

            if (document == null)
                return false;

            // Basic Fields Update
            document.EmployeeId = dto.EmployeeId;
            document.DocumentType = dto.DocumentType;
            document.IsVerified = dto.IsVerified;
            document.Remarks = dto.Remarks;

            // New File Uploaded
            if (dto.File != null && dto.File.Length > 0)
            {
                // Validate Extension
                var extension = Path.GetExtension(dto.File.FileName);

                if (!FileHelper.IsValidExtension(extension))
                    throw new Exception("Invalid file type.");

                // Validate Size
                if (!FileHelper.IsValidFileSize(dto.File))
                    throw new Exception("Maximum allowed file size is 5 MB.");

                // Delete Old File
                if (!string.IsNullOrWhiteSpace(document.FilePath))
                {
                    var oldFile = Path.Combine(
                        _environment.WebRootPath,
                        document.FilePath.TrimStart('/')
                            .Replace("/", Path.DirectorySeparatorChar.ToString()));

                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }
                }

                // Upload Folder
                var uploadFolder = Path.Combine(
                    _environment.WebRootPath,
                    "Uploads",
                    "Documents");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Generate New File Name
                var uniqueFileName =
                    FileHelper.GenerateUniqueFileName(dto.File.FileName);

                var newFilePath =
                    Path.Combine(uploadFolder, uniqueFileName);

                // Save New File
                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await dto.File.CopyToAsync(stream);
                }

                // Update Entity
                document.FileName = dto.File.FileName;
                document.FilePath = "/Uploads/Documents/" + uniqueFileName;
                document.FileExtension = extension;
                document.FileSize = dto.File.Length;
                document.UploadedDate = DateTime.Now;
            }

            await _repository.Update(document);

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var document = await _repository.GetById(id);

            if (document == null)
                return false;

            if (!string.IsNullOrWhiteSpace(document.FilePath))
            {
                var fullPath = Path.Combine(
                    _environment.WebRootPath,
                    document.FilePath.TrimStart('/')
                        .Replace("/", Path.DirectorySeparatorChar.ToString()));

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            return await _repository.Delete(id);
        }
    }
}