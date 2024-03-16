using DentalOffice_BE.Common;
using DentalOffice_BE.Common.Utility;
using DentalOffice_BE.Data;
using DentalOffice_BE.Services.Interfaces;
using DentalOffice_BE.Services.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public async Task<SectionViewModel> GetByRoute(string route)
    {
        var query = _context.Sections.AsQueryable();

        query = query.Include(_ => _.SubSections).Include(_ => _.Configuration);

        query = query.Where(_ => _.Route == '/' + route);

        var data = await query.FirstOrDefaultAsync();

        Validate.ThrowIfNull(data);

        return data.MapViewModelFromDto();
    }
}
