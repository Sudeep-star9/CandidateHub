using AutoMapper;
using CandidateHub.DTOs;
using CandidateHub.Interfaces;
using CandidateHub.Models;
using CandidateHub.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CandidateHub.Tests.Services
{
    public class CandidateServiceTests
    {
        private readonly Mock<ICandidateRepository> _mockCandidateRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMemoryCache> _mockCache;
        private readonly CandidateService _service;

        public CandidateServiceTests()
        {
            _mockCandidateRepository = new Mock<ICandidateRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockCache = new Mock<IMemoryCache>();
            _service = new CandidateService(_mockCandidateRepository.Object, _mockMapper.Object, _mockCache.Object);
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsFromCache_WhenCandidateIsCached()
        {
            var email = "sudeep@example.com";
            var candidateDto = new CandidateDto { Email = email, FirstName = "Sudeep", LastName = "Adhikari" };

            // Setup cache to return the cached value
            _mockCache.Setup(c => c.TryGetValue(email, out candidateDto)).Returns(true);

            var result = await _service.GetByEmailAsync(email);

            Assert.Equal(email, result.Email);
            _mockCandidateRepository.Verify(repo => repo.GetByEmailAsync(email), Times.Never);
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsCandidate_WhenFound()
        {
            var email = "sudeep@example.com";
            var candidate = new Candidate { Email = email, FirstName = "Sudeep", LastName = "Adhikari" };
            var candidateDto = new CandidateDto { Email = email, FirstName = "Sudeep", LastName = "Adhikari" };

            // Setup repository and mapper
            _mockCandidateRepository.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync(candidate);
            _mockMapper.Setup(mapper => mapper.Map<CandidateDto>(candidate)).Returns(candidateDto);

            var result = await _service.GetByEmailAsync(email);

            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsNull_WhenNotFound()
        {
            var email = "sudeep@example.com";
            _mockCandidateRepository.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync((Candidate?)null);

            var result = await _service.GetByEmailAsync(email);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ReturnsCandidateDto_WhenAddedSuccessfully()
        {
            var candidateDto = new CreateCandidateDto { Email = "sudeep@example.com", FirstName = "Sudeep", LastName = "Adhikari" };
            var candidateModel = new Candidate { Email = candidateDto.Email, FirstName = candidateDto.FirstName, LastName = candidateDto.LastName };

            // Setup repository and mapper for adding a new candidate
            _mockMapper.Setup(mapper => mapper.Map<Candidate>(candidateDto)).Returns(candidateModel);
            _mockCandidateRepository.Setup(repo => repo.GetByEmailAsync(candidateDto.Email)).ReturnsAsync((Candidate?)null);

            var addedCandidateModel = new Candidate { Email = "sudeep@example.com", FirstName = "Sudeep", LastName = "Adhikari" };

            _mockCandidateRepository.Setup(repo => repo.AddAsync(candidateModel)).ReturnsAsync(addedCandidateModel);

            var result = await _service.AddAsync(candidateDto);

            Assert.NotNull(result);
            Assert.Equal(candidateDto.Email, result.Email);
        }

        [Fact]
        public async Task AddAsync_ReturnsNull_WhenEmailAlreadyExists()
        {
            var candidateDto = new CreateCandidateDto { Email = "sudeep@example.com", FirstName = "Sudeep", LastName = "Adhikari" };

            // Setup existing candidate scenario
            var existingCandidateModel = new Candidate { Email = candidateDto.Email };

            _mockCandidateRepository.Setup(repo => repo.GetByEmailAsync(candidateDto.Email)).ReturnsAsync(existingCandidateModel);

            var result = await _service.AddAsync(candidateDto);

            Assert.Null(result); 
        }
    }
}