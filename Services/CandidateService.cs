using CandidateHub.DTOs;
using CandidateHub.Interfaces;
using CandidateHub.Models;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CandidateHub.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public CandidateService(ICandidateRepository candidateRepository, IMapper mapper, IMemoryCache cache)
        {
            _candidateRepository = candidateRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<CandidateDto> GetByEmailAsync(string email)
        {
            // Check if candidate is in the cache
            if (_cache.TryGetValue(email, out CandidateDto cachedCandidate))
            {
                return cachedCandidate;
            }

            // Otherwise, fetch from the database and cache it
            var candidate = await _candidateRepository.GetByEmailAsync(email);
            if (candidate == null)
                return null;

            var candidateDto = _mapper.Map<CandidateDto>(candidate);

            // Cache candidate data for 10 minutes
            _cache.Set(email, candidateDto, TimeSpan.FromMinutes(10));

            return candidateDto;
        }

        public async Task<List<CandidateDto>> GetAllAsync(int pageIndex, int pageSize)
        {
            // Define the cache key for all candidates
            var cacheKey = $"candidates_page_{pageIndex}_size_{pageSize}";

            // Try to get the list of candidates from cache
            if (_cache.TryGetValue(cacheKey, out List<CandidateDto> cachedCandidates))
            {
                return cachedCandidates;
            }

          
            var candidates = await _candidateRepository.GetAllAsync(pageIndex, pageSize);
            var candidatesDto = _mapper.Map<List<CandidateDto>>(candidates);

            // Cache the list for 10 minutes
            _cache.Set(cacheKey, candidatesDto, TimeSpan.FromMinutes(10));

            return candidatesDto;
        }

        public async Task<CandidateDto> AddAsync(CandidateDto candidateDto)
        {
            var existingCandidate = await _candidateRepository.GetByEmailAsync(candidateDto.Email);
            if (existingCandidate != null)
            {
                return null;
            }

            var candidate = _mapper.Map<Candidate>(candidateDto);
            var addedCandidate = await _candidateRepository.AddAsync(candidate);
            return _mapper.Map<CandidateDto>(addedCandidate);
        }

        public async Task<CandidateDto> UpdateAsync(int id, CandidateDto candidateDto)
        {
            var existingCandidate = await _candidateRepository.GetByEmailAsync(candidateDto.Email);
            if (existingCandidate == null)
            {
                return null;
            }

            var candidate = _mapper.Map<Candidate>(candidateDto);
            var updatedCandidate = await _candidateRepository.UpdateAsync(candidate);

            return _mapper.Map<CandidateDto>(updatedCandidate);
        }

        public async Task<CandidateDto> DeleteAsync(int id)
        {
            var deletedCandidate = await _candidateRepository.DeleteAsync(id);
            return deletedCandidate == null ? null : _mapper.Map<CandidateDto>(deletedCandidate);
        }
    }
}
