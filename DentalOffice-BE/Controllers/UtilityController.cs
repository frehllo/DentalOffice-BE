using Microsoft.AspNetCore.Mvc;

namespace DentalOffice_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class UtilityController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { status = "Server is up and running!" });
        }
    }
}
