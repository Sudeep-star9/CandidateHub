using CandidateHub.DTOs;
using CandidateHub.Interfaces;
using CandidateHub.ValidationModelAttribute; 
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CandidateHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService _candidateService;

        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

     
        [HttpGet("{email}")]
        public async Task<IActionResult> GetCandidateByEmail(string email)
        {
            var candidate = await _candidateService.GetByEmailAsync(email);

            if (candidate == null)
            {
                return NotFound(new { message = "Candidate not found" });
            }

            return Ok(candidate);
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAllCandidates([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var candidates = await _candidateService.GetAllAsync(pageIndex, pageSize);

            if (candidates == null || candidates.Count == 0)
            {
                return NotFound(new { message = "No candidates found" });
            }

            return Ok(candidates);
        }

        
        [HttpPost]
        [ValidateModel]  
        public async Task<IActionResult> AddCandidate([FromBody] CandidateDto candidateDto)
        {
            if (candidateDto == null)
            {
                return BadRequest(new { message = "Invalid candidate data" });
            }

           
            var existingCandidate = await _candidateService.GetByEmailAsync(candidateDto.Email);
            if (existingCandidate != null)
            {
                return BadRequest(new { message = "Email already exists" });
            }

            var newCandidate = await _candidateService.AddAsync(candidateDto);
            if (newCandidate == null)
            {
                return BadRequest(new { message = "Failed to add candidate" });
            }

            return CreatedAtAction(nameof(GetCandidateByEmail), new { email = newCandidate.Email }, newCandidate);
        }


        
        [HttpPut("{id}")]
        [ValidateModel]  
        public async Task<IActionResult> UpdateCandidate(int id, [FromBody] CandidateDto candidateDto)
        {
            if (candidateDto == null)
            {
                return BadRequest(new { message = "Invalid candidate data" });
            }

            var updatedCandidate = await _candidateService.UpdateAsync(id, candidateDto);
            if (updatedCandidate == null)
            {
                return NotFound(new { message = "Candidate not found or email is already in use" });
            }

            return Ok(updatedCandidate);
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            var deletedCandidate = await _candidateService.DeleteAsync(id);

            if (deletedCandidate == null)
            {
                return NotFound(new { message = "Candidate not found" });
            }

            return Ok(new { message = "Candidate deleted successfully", candidate = deletedCandidate });
        }
    }
}
