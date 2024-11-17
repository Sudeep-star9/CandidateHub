using CandidateHub.Models;
using CandidateHub.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CandidateHub.Data;

namespace CandidateHub.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;

        public CandidateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Candidate> GetByEmailAsync(string email)
        {
            return await _context.Candidates.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<List<Candidate>> GetAllAsync(int pageIndex, int pageSize)
        {
            return await _context.Candidates
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Candidate> AddAsync(Candidate candidate)
        {
            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();
            return candidate;
        }

        public async Task<Candidate> UpdateAsync(string email, Candidate candidate)
        {
            
            var existingCandidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Email == email);

            if (existingCandidate != null)
            {
            
                _context.Entry(existingCandidate).CurrentValues.SetValues(candidate);

                await _context.SaveChangesAsync();
                return existingCandidate;
            }

            return null; 
        }

        public async Task<Candidate> DeleteAsync(int id)
        {
            var candidate = await _context.Candidates.FirstOrDefaultAsync(x=> x.Id==id);
            if (candidate != null)
            {
                _context.Candidates.Remove(candidate);
                await _context.SaveChangesAsync();
            }
            return candidate;
        }
    }
}
