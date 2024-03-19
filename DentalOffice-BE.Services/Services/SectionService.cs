using DentalOffice_BE.Common;
using DentalOffice_BE.Common.Utility;
using DentalOffice_BE.Data;
using DentalOffice_BE.Services.Interfaces;
using DentalOffice_BE.Services.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DentalOffice_BE.Services.Services;

public class SectionService(DBContext _context) : ISectionService
{
    public async Task<IEnumerable<SectionViewModel>> GetList()
    {
        var query = _context.Sections.AsQueryable();

        query = query.Include(_ => _.SubSections).Include(_ => _.Configuration);

        query = query.Where(_ => _.SectionId == null);

        var data = await query.ToListAsync();

        return data.Select(_ => _.MapViewModelFromDto());
    }

    public async Task<SectionViewModel> GetByRoute(string apiString)
    {
        var query = _context.Sections.AsQueryable();

        query = query.Include(_ => _.SubSections).Include(_ => _.Configuration);

        query = query.Where(_ => _.Route == '/' + apiString);
        
        var data = await query.FirstOrDefaultAsync();

        Validate.ThrowIfNull(data);

        return data.MapViewModelFromDto();
    }

    public async Task<dynamic> GetSingleData(string id, string apiString)
    {
        var query = GetSingleQuery(apiString, id);

        return await query.FirstOrDefaultAsync() ?? null!;
    }

    public async Task<IEnumerable<dynamic>> GetAllData(string apiString)
    {
        var query = GetQuery(apiString);

        return await query.ToListAsync();
    }

    private IQueryable<dynamic> GetSingleQuery(string apiString, string id)
    {
        if (apiString.Contains("materials"))
        {
            string[] parts = apiString.Split("-");
            if (parts.Length > 1)
            {
                long type = long.Parse(parts[1]);
                return _context.Materials.AsQueryable().Where(_ => _.MaterialTypeId == type).Where(_ => _.Id == long.Parse(id));
            }
        }

        if (apiString.Contains("lots"))
        {
            string[] parts = apiString.Split("-");
            if (parts.Length > 1)
            {
                long type = long.Parse(parts[1]);
                return _context.Lots.AsQueryable().Where(_ => _.MaterialId == type).Where(_ => _.Id == long.Parse(id));
            }
        }

        IQueryable<dynamic> query = apiString switch
        {
            "studios" => _context.Studios.AsQueryable().Where(_ => _.Id == long.Parse(id)),
            "colors" => _context.Colors.AsQueryable().Where(_ => _.Id == id),
            "semiproducts" => _context.SemiProducts.AsQueryable().Where(_ => _.Id == long.Parse(id)),
            "risks" => _context.Risks.AsQueryable().Where(_ => _.Id == long.Parse(id)),
            "stages" => _context.Stages.AsQueryable().Where(_ => _.Id == long.Parse(id)),
            "modules" => _context.Modules.AsQueryable().Where(_ => _.Id == long.Parse(id)),
            _ => throw new Exception("ApiString not allowed")
        };

        return query;
    }

    private IQueryable<dynamic> GetQuery(string apiString)
    {
        if (apiString.Contains("materials"))
        {
            string[] parts = apiString.Split("-");
            if (parts.Length > 1)
            {
                long type = long.Parse(parts[1]);
                return _context.Materials.AsQueryable().Where(_ => _.MaterialTypeId == type);
            }
        }

        if (apiString.Contains("lots"))
        {
            string[] parts = apiString.Split("-");
            if (parts.Length > 1)
            {
                long type = long.Parse(parts[1]);
                return _context.Lots.AsQueryable().Where(_ => _.MaterialId == type);
            }
        }

        IQueryable<dynamic> query = apiString switch
        {
            "studios" => _context.Studios.AsQueryable(),
            "colors" => _context.Colors.AsQueryable(),
            "semiproducts" => _context.SemiProducts.AsQueryable(),
            "risks" => _context.Risks.AsQueryable(),
            "stages" => _context.Stages.AsQueryable(),
            "modules" => _context.Modules.AsQueryable(),
            _ => throw new Exception("ApiString not allowed")
        };

        return query;
    }

    public async Task InsertData(string apiString, object data)
    {
        var type = apiString.Contains("-") ? apiString.Split("-")[0] : apiString;

        var serialized = JsonConvert.SerializeObject(data);

        switch (type)
        {
            case "studios":
                StudioDto? toSend = JsonConvert.DeserializeObject<StudioDto>(data.ToString());
                _context.Add(toSend);
                break;
            case "colors":
                _context.Add((ColorDto)data);
                break;
            case "semiproduct":
                _context.Add((SemiProductDto)data);
                break;
            case "risks":
                _context.Add((RiskDto)data);
                break;
            case "stages":
                _context.Add((StageDto)data);
                break;
            case "modules":
                _context.Add((ModuleDto)data);
                break;
            case "lots":
                _context.Add((LotDto)data);
                break;
            case "materials":
                _context.Add((MaterialDto)data);
                break;
            default: throw new Exception("Route not allowed");
        }

        await _context.SaveChangesAsync();
    }
}
