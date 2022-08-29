using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerAPIServices.Controllers;

[Route("api/[controller]")]
public class CustomersController : Controller
{
    [Authorize]
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new[] { "Catcher Wong", "James Li" };
    }

    [HttpGet("{id}")]
    public string Get(int id)
    {
        return $"Catcher Wong - {id}";
    }
}