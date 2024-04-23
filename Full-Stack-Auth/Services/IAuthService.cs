using Full_Stack_Auth.DTOs;
using Full_Stack_Auth.Models;

namespace Full_Stack_Auth.Services;

public interface IAuthService
{
    public Task<AuthDTO> GenerateToken(AppUser user);
}