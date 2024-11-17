using CandidateHub.DTOs.UserDto;
using Microsoft.AspNetCore.Identity;

namespace CandidateHub.Interfaces
{
    public interface IAuthRepository
    {
        Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    }
}
