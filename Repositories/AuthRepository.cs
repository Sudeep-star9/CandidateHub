using CandidateHub.DTOs.UserDto;
using CandidateHub.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CandidateHub.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthRepository(UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email,  
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                // Assign default role
                var roleExist = await _userManager.IsInRoleAsync(user, "Admin");
                if (!roleExist)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

            return result;
        }


        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var token = _tokenRepository.CreateJWTToken(user, roles.ToList());
                return new LoginResponseDto { JwtToken = token };
            }
            return null;
        }
    }
}
