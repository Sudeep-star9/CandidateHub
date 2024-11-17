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

   

    [Fact]
    public async Task GetCandidateByEmail_ReturnsNotFound_WhenCandidateDoesNotExist()
    {
        // Arrange
        var email = "nonexistent@example.com";
        _mockCandidateService.Setup(s => s.GetByEmailAsync(email))
                             .ReturnsAsync((CandidateDto)null);

        // Act
        var result = await _controller.GetCandidateByEmail(email);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Candidate not found", ((dynamic)notFoundResult.Value).message);
    }

   

    [Fact]
    public async Task GetAllCandidates_ReturnsNotFound_WhenNoCandidatesExist()
    {
        // Arrange
        _mockCandidateService.Setup(s => s.GetAllAsync(1, 10)).ReturnsAsync(new List<CandidateDto>());

        // Act
        var result = await _controller.GetAllCandidates(1, 10);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No candidates found", ((dynamic)notFoundResult.Value).message);
    }

   

    [Fact]
    public async Task AddCandidate_ReturnsBadRequest_WhenEmailAlreadyExists()
    {
        // Arrange
        var candidateDto = new CreateCandidateDto
        {
            Email = "ramsudeep@example.com",
            FirstName = "Shyam",
            LastName = "Sudeep",
            Comments = "Test comment"
        };

        _mockCandidateService.Setup(s => s.GetByEmailAsync(candidateDto.Email))
            .ReturnsAsync(new CandidateDto());

        // Act
        var result = await _controller.AddCandidate(candidateDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Email already exists", ((dynamic)badRequestResult.Value).message);
    }

    [Fact]
    public async Task UpdateCandidate_ReturnsOk_WhenUpdatedSuccessfully()
    {
        // Arrange
        string email = "updated@example.com";
        var candidateToUpdate = new CreateCandidateDto
        {
            Email = email,
            FirstName = "Updated",
            LastName = "User",
            Comments = "Updated comment"
        };
        var updatedCandidate = new CandidateDto
        {
            Email = email,
            FirstName = "Updated",
            LastName = "User",
            Comments = "Updated comment"
        };

        _mockCandidateService.Setup(s => s.UpdateAsync(email, candidateToUpdate))
            .ReturnsAsync(updatedCandidate);

        // Act
        var result = await _controller.UpdateCandidate(email, candidateToUpdate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<CandidateDto>(okResult.Value);
        Assert.Equal(email, returnValue.Email);
    }



  

    [Fact]
    public async Task DeleteCandidate_ReturnsNotFound_WhenDoesNotExist()
    {
        // Arrange
        int idNotFound = 99;

        _mockCandidateService.Setup(s => s.DeleteAsync(idNotFound))
            .ReturnsAsync((CandidateDto)null);

        // Act
        var result = await _controller.DeleteCandidate(idNotFound);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Candidate not found", ((dynamic)notFoundResult.Value).message);
    }
}