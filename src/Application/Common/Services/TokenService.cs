using Application.Common.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Common.Services;
public class TokenService
{
    public AuthenticationDto GenerateToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("inmetricsinmetrics_inmetrics");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name,"Application Name")
            }),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var validFrom = token.ValidFrom;
        var validTo = token.ValidTo;
        return new AuthenticationDto
        {
            ValidFrom = validFrom,
            ValidTo = validTo,
            Token = tokenHandler.WriteToken(token)
        };
    }
}
