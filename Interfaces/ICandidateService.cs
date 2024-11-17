using CandidateHub.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CandidateHub.Interfaces
{
    public interface ICandidateService
    {
        Task<CandidateDto> GetByEmailAsync(string email);
        Task<List<CandidateDto>> GetAllAsync(int pageIndex, int pageSize);
        Task<CandidateDto> AddAsync(CreateCandidateDto candidateDto);
        Task<CandidateDto> UpdateAsync(string email, CreateCandidateDto candidateDto); // Updated method signature
        Task<CandidateDto> DeleteAsync(int id);
    }
}