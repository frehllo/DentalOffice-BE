using DentalOffice_BE.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace DentalOffice_BE.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectionController : ControllerBase
{
    [HttpGet]
    public async IEnumerable<SectionViewModel> GetList()
    {

    }
}
