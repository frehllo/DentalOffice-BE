using DentalOffice_BE.Data;
using DentalOffice_BE.Services.Interfaces;
using DentalOffice_BE.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace DentalOffice_BE.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class ModuleController(IModuleService _service) : ControllerBase
{
    [HttpGet("configuration")]
    public async Task<ModuleFormConfiguration> GetConfiguration()
    {
        return await _service.GetConfiguration();
    }

    [HttpGet]
    public async Task<IEnumerable<ModuleDto>> GetList()
    {
        return await _service.GetList();
    }

    [HttpGet("{id}")]
    public async Task<ModuleDto> GetList(long id)
    {
        return await _service.Get(id);
    }

    [HttpPost]
    public async Task<ModuleDto> Insert(ModuleDto model)
    {
        return await _service.Insert(model);
    }

    [HttpPut("{id}")]
    public async Task<ModuleDto> Update(long id, ModuleDto model)
    {
        return await _service.Update(id, model);
    }
}
