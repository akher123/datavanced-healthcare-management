namespace Datavanced.HealthcareManagement.Services;
using Datavanced.HealthcareManagement.Data.Models;
using Datavanced.HealthcareManagement.Shared;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<string> GenerateToken(ApplicationUser user, IList<string> roles)
    {
        var token = await GenerateJwtAsync(user, roles);
        return token;
    }
    private async Task<string> GenerateJwtAsync(ApplicationUser user,IList<string> roles)
    {
        var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user, roles));
        return token;
    }
    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Issuer,
           claims: claims,
           expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiryMinutes),
           signingCredentials: signingCredentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        var encryptedToken = tokenHandler.WriteToken(token);
        return encryptedToken;
    }
    private async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user,IList<string>roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("OfficeId", user.OfficeId.ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return await Task.FromResult(claims);
    }
    private SigningCredentials GetSigningCredentials()
    {
        var secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

 
}
