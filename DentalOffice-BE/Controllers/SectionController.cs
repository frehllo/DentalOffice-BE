using DentalOffice_BE.Services.Interfaces;
using DentalOffice_BE.Services.Models;
using Microsoft.AspNetCore.Mvc;


namespace DentalOffice_BE.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class SectionController(ISectionService _service) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<SectionViewModel>> GetList()
    {
        return await _service.GetList();
    }

    [HttpGet("{route}")]
    public async Task<SectionViewModel> GetByRoute(string route)
    {
        return await _service.GetByRoute(route);
    }
}
