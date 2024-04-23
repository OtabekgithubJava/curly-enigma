using Full_Stack_Auth.DTOs;
using Full_Stack_Auth.Models;
using Full_Stack_Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Full_Stack_Auth.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<AppUser> _inManager;

    public UsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> inManager, IAuthService authService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _inManager = inManager;
        _authService = authService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromForm] RegisterDTO registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new AppUser
        {
            FullName = registerDto.FullName,
            UserName = registerDto.Email,
            Email = registerDto.Email,
            Age = registerDto.Age,
            Status = registerDto.Status
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        foreach (var role in registerDto.Roles)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<AuthDTO>> Login([FromForm] LoginDTO loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user is null)
        {
            return Unauthorized("User Not Found With This Email");
        }
        
        var test = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!test)
        {
            return Unauthorized("Password invalid");
        }

        var token = await _authService.GenerateToken(user);

        // var result = await _inManager.PasswordSignInAsync(user, loginDto.Password, false, false);
        //
        // if (!result.Succeeded)
        // {
        //     throw new Exception("Sth is wrong with signing-in process!");
        //     
        // }
        return Ok(token);
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<string>> GetAllUsers()
    {
        var result = await _userManager.Users.ToListAsync();
        
        return Ok(result);
    }
    
}