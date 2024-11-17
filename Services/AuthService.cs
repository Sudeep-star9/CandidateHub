using CandidateHub.DTOs.UserDto;
using CandidateHub.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CandidateHub.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            return await _authRepository.RegisterAsync(registerDto);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            return await _authRepository.LoginAsync(loginDto);
        }
    }
}
