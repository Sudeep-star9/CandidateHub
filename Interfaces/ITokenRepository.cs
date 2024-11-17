using CandidateHub.DTOs.UserDto;

namespace CandidateHub.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJWTToken(ApplicationUser user, List<string> roles);
    }
}
