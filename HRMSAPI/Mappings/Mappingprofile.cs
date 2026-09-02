using AutoMapper;
using Entites.Model;
using HRMSAPI.DTOs;
using HRMSAPI.DTOs.EmployeeDocument;

namespace HRMSAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Employee → Response DTO
            CreateMap<Employee, EmployeeResponseDto>()
     .ForMember(dest => dest.Department,
         opt => opt.MapFrom(src => src.Department.Name))

     .ForMember(dest => dest.Designation,
         opt => opt.MapFrom(src => src.Designation.Title))

     .ForMember(dest => dest.ShiftName,
         opt => opt.MapFrom(src =>
             src.Shift != null
                 ? src.Shift.ShiftName
                 : null));

            // Create DTO → Employee
            CreateMap<CreateEmployeeDto, Employee>();

            // Update DTO → Employee
            CreateMap<UpdateEmployeeDto, Employee>();
            // =========================
            // Employee Document Mapping
            // =========================

            CreateMap<EmployeeDocumentCreateDto, EmployeeDocument>();

            CreateMap<EmployeeDocumentUpdateDto, EmployeeDocument>();

            CreateMap<EmployeeDocument, EmployeeDocumentResponseDto>()
                .ForMember(dest => dest.EmployeeName,
                    opt => opt.MapFrom(src =>
                        src.Employee.FirstName + " " + src.Employee.LastName));
            CreateMap<ITAsset, ITAssetResponseDto>()
    .ForMember(dest => dest.EmployeeName,
        opt => opt.MapFrom(src =>
            src.Employee.FirstName + " " + src.Employee.LastName));

            CreateMap<CreateITAssetDto, ITAsset>();

            CreateMap<UpdateITAssetDto, ITAsset>();
            // =========================
            // SHIFT
            // =========================

            CreateMap<CreateShiftDto, Shift>();

            CreateMap<UpdateShiftDto, Shift>();

            CreateMap<Shift, ShiftResponseDto>();
            CreateMap<Shift, ShiftDropdownDto>()
    .ForMember(
        dest => dest.Name,
        opt => opt.MapFrom(src => src.ShiftName));
        }
    }
}