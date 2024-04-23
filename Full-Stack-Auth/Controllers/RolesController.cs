using Full_Stack_Auth.DTOs;
using Full_Stack_Auth.Models;
using Full_Stack_Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Full_Stack_Auth.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ResponseDTO>> CreateRole(RoleDTO role)
    {
        var result = await _roleManager.FindByNameAsync(role.RoleName);

        if (result == null)
        {
            await _roleManager.CreateAsync(new IdentityRole(role.RoleName));
            return Ok(new ResponseDTO
            {
                Message = "Role Created",
                IsSuccessful = true,
                StatusCode = 201 
            });
        }
        return Ok(new ResponseDTO
        {
            Message = "Role Cannot Be Created",
            StatusCode = 403
        });
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<IdentityRole>>> GetAllRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();

        return Ok(roles);
    }
    
}