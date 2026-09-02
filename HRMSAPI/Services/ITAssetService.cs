using AutoMapper;
using Entites.Model;
using Helpers;
using HRMSAPI.DTOs;
using HRMSAPI.Repository.Interfaces;
using HRMSAPI.Services.Interfaces;
using Microsoft.Identity.Client;

namespace HRMSAPI.Services
{
    public class ITAssetService : IITAssetService
    {
        private readonly IITAssetRepository _repository;
        private readonly IMapper _mapper;

        public ITAssetService(
            IITAssetRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================

        public async Task<List<ITAssetResponseDto>> GetAll(PaginationDto paginationDto)
        {
            var assets = await _repository.GetAll(paginationDto);

            return _mapper.Map<List<ITAssetResponseDto>>(assets);
        }

        // =========================
        // GET MY ASSETS
        // =========================

        public async Task<List<ITAssetResponseDto>> GetMyAssets(
            int employeeId,
            PaginationDto paginationDto)
        {
            var assets = await _repository.GetMyAssets(employeeId, paginationDto);

            return _mapper.Map<List<ITAssetResponseDto>>(assets);
        }

        // =========================
        // GET BY ID
        // =========================

        public async Task<ITAssetResponseDto?> GetById(int id)
        {
            var asset = await _repository.GetById(id);

            if (asset == null)
                return null;

            return _mapper.Map<ITAssetResponseDto>(asset);
        }

        // =========================
        // CREATE
        // =========================

        public async Task<ITAssetResponseDto> Create(CreateITAssetDto dto)
        {
            var asset = _mapper.Map<ITAsset>(dto);

            var result = await _repository.Create(asset);

            return _mapper.Map<ITAssetResponseDto>(result);
        }

        // =========================
        // UPDATE
        // =========================

        public async Task<bool> Update(int id, UpdateITAssetDto dto)
        {
            var asset = _mapper.Map<ITAsset>(dto);

            asset.Id = id;

            return await _repository.Update(asset);
        }

        // =========================
        // DELETE
        // =========================

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }
    }
}