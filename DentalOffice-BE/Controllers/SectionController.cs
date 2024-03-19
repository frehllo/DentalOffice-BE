﻿using DentalOffice_BE.Data;
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

    [HttpGet("data/{apiString}")]
    public async Task<IEnumerable<dynamic>> GetAllData(string apiString)
    {
        return await _service.GetAllData(apiString);
    }

    [HttpGet("data/{id}/{apiString}")]
    public async Task<dynamic> GetSingleData(string id, string apiString)
    {
        return await _service.GetSingleData(id, apiString);
    }

    [HttpPost("data/{apiString}")]
    public async Task InsertData(string apiString, [FromBody] object data)
    {
        await _service.InsertData(apiString, data);
    }
}
