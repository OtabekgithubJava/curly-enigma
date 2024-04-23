namespace Full_Stack_Auth.DTOs;

public class ResponseDTO
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public bool IsSuccessful { get; set; } = false;
}