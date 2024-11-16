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
            return await _context.candidates.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<List<Candidate>> GetAllAsync(int pageIndex, int pageSize)
        {
            return await _context.candidates
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Candidate> AddAsync(Candidate candidate)
        {
            _context.candidates.Add(candidate);
            await _context.SaveChangesAsync();
            return candidate;
        }

        public async Task<Candidate> UpdateAsync(Candidate candidate)
        {
            _context.candidates.Update(candidate);
            await _context.SaveChangesAsync();
            return candidate;
        }

        public async Task<Candidate> DeleteAsync(int id)
        {
            var candidate = await _context.candidates.FindAsync(id);
            if (candidate != null)
            {
                _context.candidates.Remove(candidate);
                await _context.SaveChangesAsync();
            }
            return candidate;
        }
    }
}
