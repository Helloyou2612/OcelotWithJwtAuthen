using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AuthServer.Controllers
{
    [Route("api/[controller]/")]
    public class DemoController : Controller
    {
        private IOptions<Audience> _settings;

        public DemoController(IOptions<Audience> settings)
        {
            this._settings = settings;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get()
        {
            return Ok("hello Lam!");
        }
    }
}
