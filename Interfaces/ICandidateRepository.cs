using CandidateHub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CandidateHub.Interfaces
{
    public interface ICandidateRepository
    {
        Task<Candidate> GetByEmailAsync(string email);
        Task<List<Candidate>> GetAllAsync(int pageIndex, int pageSize);
        Task<Candidate> AddAsync(Candidate candidate);
        Task<Candidate> UpdateAsync(Candidate candidate);
        Task<Candidate> DeleteAsync(int id);
    }
}
