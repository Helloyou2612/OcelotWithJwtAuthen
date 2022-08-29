using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthServer.Controllers;

[Route("api/[controller]/")]
public class AuthController : Controller
{
    private readonly IOptions<Audience> _settings;

    public AuthController(IOptions<Audience> settings)
    {
        _settings = settings;
    }

    [HttpGet]
    public IActionResult Get(string name, string pwd)
    {
        if (name == "catcher" && pwd == "123")
        {
            var now = DateTime.UtcNow;

            var claims = new[]
            {
                new(JwtRegisteredClaimNames.Sub, name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Value.Secret));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = _settings.Value.Iss,
                ValidateAudience = true,
                ValidAudience = _settings.Value.Aud,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true
            };

            var jwt = new JwtSecurityToken(
                _settings.Value.Iss,
                _settings.Value.Aud,
                claims,
                now,
                now.Add(TimeSpan.FromMinutes(2)),
                new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var responseJson = new
            {
                access_token = encodedJwt,
                expires_in = (int)TimeSpan.FromMinutes(2).TotalSeconds
            };

            return Json(responseJson);
        }

        return Json("");
    }
}

public class Audience
{
    public string Secret { get; set; }
    public string Iss { get; set; }
    public string Aud { get; set; }
}