namespace Full_Stack_Auth.DTOs;

public class UpdateUserDTO
{
    public string FullName { get; set; }
    public int Age { get; set; }
    public string Status { get; set; }
    public string Email { get; set; } 
    public IList<string> Roles { get; set; } 
}