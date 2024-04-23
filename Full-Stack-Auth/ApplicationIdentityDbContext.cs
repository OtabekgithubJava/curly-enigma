using Full_Stack_Auth.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Full_Stack_Auth;

public class ApplicationIdentityDbContext: IdentityDbContext<AppUser>
{
    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
        : base(options)
    {
        
    }
}