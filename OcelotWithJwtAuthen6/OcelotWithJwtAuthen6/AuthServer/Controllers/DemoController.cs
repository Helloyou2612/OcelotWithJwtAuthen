using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AuthServer.Controllers;

[Route("api/[controller]/")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DemoController : Controller
{
    private IOptions<Audience> _settings;

    public DemoController(IOptions<Audience> settings)
    {
        _settings = settings;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("hello Lam!");
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public IActionResult Get(int id)
    {
        return Ok($"hello Lam - {id}");
    }
}