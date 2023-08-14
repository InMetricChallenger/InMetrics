namespace Application.Common.Models.DTOs;
public class AuthenticationDto
{
    public string Token { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }
}
