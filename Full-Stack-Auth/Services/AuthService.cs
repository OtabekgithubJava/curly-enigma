using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Full_Stack_Auth.DTOs;
using Full_Stack_Auth.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Full_Stack_Auth.Services;

public class AuthService: IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<AuthDTO> GenerateToken(AppUser user)
    {
        if (user is null)
        {
            return new AuthDTO()
            {
                Message = "User Is Null",
                StatusCode = 404
            };
        }
        
        var tokenHandler = new JwtSecurityTokenHandler();

        // var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWTSetting").GetSection("SecretKey").Value);
        var key = Encoding.ASCII.GetBytes(_configuration["JWTSettings:SecretKey"]!);

        var roles = await _userManager.GetRolesAsync(user);

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Name, user.FullName),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new(JwtRegisteredClaimNames.NameId, user.Id),
            new(JwtRegisteredClaimNames.Aud, _configuration["JWTSettings:ValidAudience"]!),
            new(JwtRegisteredClaimNames.Iss, _configuration["JWTSettings:ValidIssuer"]!),
            new(JwtRegisteredClaimNames.Exp, _configuration["JwtSettings:ExpireDate"]!),
            new(ClaimTypes.Role, "Admin")
        ];
        
        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:ExpireDate"])),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthDTO()
        {
            Token = tokenHandler.WriteToken(token),
            Message = "Token Created Successfully",
            StatusCode = 200,
            IsSuccessful = true
        };
    }
}