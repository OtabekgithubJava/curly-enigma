namespace Full_Stack_Auth.DTOs;

public class AuthDTO
{
    public string Token { get; set; } 
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public bool IsSuccessful { get; set; } = false;
}