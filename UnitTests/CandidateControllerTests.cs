using CandidateHub.Controllers;
using CandidateHub.DTOs;
using CandidateHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class CandidateControllerTests
{
    private readonly Mock<ICandidateService> _mockCandidateService;
    private readonly CandidateController _controller;

    public CandidateControllerTests()
    {
        _mockCandidateService = new Mock<ICandidateService>();
        _controller = new CandidateController(_mockCandidateService.Object);
    }

    // Test for GetCandidateByEmail (Success case)
    [Fact]
    public async Task GetCandidateByEmail_ReturnsOk_WhenCandidateExists()
    {
        // Arrange
        var email = "sudeepram@example.com";
        var candidateDto = new CandidateDto { Email = email, FirstName = "Sudeep", LastName = "Ram" };
        _mockCandidateService.Setup(service => service.GetByEmailAsync(email))
                             .ReturnsAsync(candidateDto);

        // Act
        var result = await _controller.GetCandidateByEmail(email);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<CandidateDto>(okResult.Value);
        Assert.Equal(email, returnValue.Email);
    }

    // Test for GetCandidateByEmail (Failure case - Candidate not found)
    [Fact]
    public async Task GetCandidateByEmail_ReturnsNotFound_WhenCandidateDoesNotExist()
    {
        // Arrange
        var email = "nonexistent@example.com";
        _mockCandidateService.Setup(service => service.GetByEmailAsync(email))
                             .ReturnsAsync((CandidateDto?)null);

        // Act
        var result = await _controller.GetCandidateByEmail(email);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    // Test for GetAllCandidates (Success case)
    [Fact]
    public async Task GetAllCandidates_ReturnsOk_WhenCandidatesExist()
    {
        // Arrange
        var candidates = new List<CandidateDto>
        {
            new CandidateDto { Email = "shyamhari@example.com", FirstName = "Shyam", LastName = "Hari" },
            new CandidateDto { Email = "ramsudeep@example.com", FirstName = "Ram", LastName = "Sudeep" }
        };
        _mockCandidateService.Setup(service => service.GetAllAsync(1, 10)).ReturnsAsync(candidates);

        // Act
        var result = await _controller.GetAllCandidates(1, 10);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<CandidateDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    // Test for GetAllCandidates (Failure case - No candidates)
    [Fact]
    public async Task GetAllCandidates_ReturnsNotFound_WhenNoCandidatesExist()
    {
        // Arrange
        _mockCandidateService.Setup(service => service.GetAllAsync(1, 10)).ReturnsAsync(new List<CandidateDto>());

        // Act
        var result = await _controller.GetAllCandidates(1, 10);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    // Test for AddCandidate (Success case)
    [Fact]
    public async Task AddCandidate_ReturnsCreatedAtAction_WhenCandidateAddedSuccessfully()
    {
        // Arrange
        var candidateDto = new CandidateDto { Email = "harishyam@example.com", FirstName = "Hari", LastName = "Shyam" };
        var newCandidate = new CandidateDto { Email = "harishyam@example.com", FirstName = "Shyam", LastName = "Hari" };
        _mockCandidateService.Setup(service => service.AddAsync(candidateDto))
                             .ReturnsAsync(newCandidate);

        // Act
        var result = await _controller.AddCandidate(candidateDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<CandidateDto>(createdAtActionResult.Value);
        Assert.Equal("harishyam@example.com", returnValue.Email);
    }

    // Test for AddCandidate (Failure case - Email already exists)
    [Fact]
    public async Task AddCandidate_ReturnsBadRequest_WhenEmailAlreadyExists()
    {
        // Arrange
        var candidateDto = new CandidateDto { Email = "ramsudeep@example.com", FirstName = "Shyam", LastName = "Sudeep" };
        _mockCandidateService.Setup(service => service.GetByEmailAsync(candidateDto.Email))
                             .ReturnsAsync(candidateDto);

        // Act
        var result = await _controller.AddCandidate(candidateDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Email already exists", ((dynamic)badRequestResult.Value).message);
    }

    // Test for UpdateCandidate (Success case)
    [Fact]
    public async Task UpdateCandidate_ReturnsOk_WhenCandidateUpdatedSuccessfully()
    {
        // Arrange
        var id = 1;
        var candidateDto = new CandidateDto { Email = "updated@example.com", FirstName = "Updated", LastName = "User" };
        var updatedCandidate = new CandidateDto { Email = "updated@example.com", FirstName = "Updated", LastName = "User" };
        _mockCandidateService.Setup(service => service.UpdateAsync(id, candidateDto))
                             .ReturnsAsync(updatedCandidate);

        // Act
        var result = await _controller.UpdateCandidate(id, candidateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<CandidateDto>(okResult.Value);
        Assert.Equal("updated@example.com", returnValue.Email);
    }

    // Test for UpdateCandidate (Failure case - Candidate not found)
    [Fact]
    public async Task UpdateCandidate_ReturnsNotFound_WhenCandidateNotFound()
    {
        // Arrange
        var id = 99;
        var candidateDto = new CandidateDto { Email = "notfound@example.com", FirstName = "Nonexistent", LastName = "User" };
        _mockCandidateService.Setup(service => service.UpdateAsync(id, candidateDto))
                             .ReturnsAsync((CandidateDto?)null);

        // Act
        var result = await _controller.UpdateCandidate(id, candidateDto);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    // Test for DeleteCandidate (Success case)
    [Fact]
    public async Task DeleteCandidate_ReturnsOk_WhenCandidateDeletedSuccessfully()
    {
        // Arrange
        var id = 1;
        var deletedCandidate = new CandidateDto { Email = "deleted@example.com", FirstName = "Deleted", LastName = "User" };
        _mockCandidateService.Setup(service => service.DeleteAsync(id)).ReturnsAsync(deletedCandidate);

        // Act
        var result = await _controller.DeleteCandidate(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<dynamic>(okResult.Value);
        Assert.Equal("Candidate deleted successfully", returnValue.message);
    }

    // Test for DeleteCandidate (Failure case - Candidate not found)
    [Fact]
    public async Task DeleteCandidate_ReturnsNotFound_WhenCandidateDoesNotExist()
    {
        // Arrange
        var id = 99;
        _mockCandidateService.Setup(service => service.DeleteAsync(id)).ReturnsAsync((CandidateDto?)null);

        // Act
        var result = await _controller.DeleteCandidate(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
