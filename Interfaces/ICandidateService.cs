using CandidateHub.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CandidateHub.Interfaces
{
    public interface ICandidateService
    {
        Task<CandidateDto> GetByEmailAsync(string email);
        Task<List<CandidateDto>> GetAllAsync(int pageIndex, int pageSize);
        Task<CandidateDto> AddAsync(CandidateDto candidateDto);
        Task<CandidateDto> UpdateAsync(int id, CandidateDto candidateDto);
        Task<CandidateDto> DeleteAsync(int id);
    }
}
