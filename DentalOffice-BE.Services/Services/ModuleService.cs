using DentalOffice_BE.Common;
using DentalOffice_BE.Common.Utility;
using DentalOffice_BE.Data;
using DentalOffice_BE.Services.Interfaces;
using DentalOffice_BE.Services.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DentalOffice_BE.Services.Services;

public class ModuleService(DBContext _context) : IModuleService
{
    public async Task<IEnumerable<ModuleDto>> GetList()
    {
        return await _context.Modules.Include(_ => _.Studio).ToListAsync();
    }

    public async Task<ModuleDto> Get(long id)
    {
        var entityDB = await _context.Modules.Include(_ => _.Processes).Include(_ => _.Studio).Where(_ => _.Id == id).FirstOrDefaultAsync();
        Validate.ThrowIfNull(entityDB);

        return entityDB;
    }

    public async Task<ModuleDto> Insert(ModuleDto model)
    {
        _context.Add(model);
        await _context.SaveChangesAsync();

        return model;
    }

    public Task<ModuleDto> Update(ModuleDto model)
    {
        throw new NotImplementedException();
    }

    public async Task<ModuleFormConfiguration> GetConfiguration()
    {
        ModuleFormConfiguration? moduleConfig = null;

        using (StreamReader r = new StreamReader("../DentalOffice-BE.Services/json/module.json"))
        {
            string json = r.ReadToEnd();
            moduleConfig = JsonConvert.DeserializeObject<ModuleFormConfiguration>(json);
            Validate.ThrowIfNull(moduleConfig);
        }

        if (moduleConfig.PersonalDataForm != null && moduleConfig.PersonalDataForm.Count > 0)
        {
            var studios = await _context.Studios.OrderByDescending(_ => _.UpdateDate).ToDictionaryAsync(_ => _.Name, _ => _.Id);

            foreach (var s in studios)
            {
                moduleConfig.PersonalDataForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "studioId")?.Props?.Options?.Add(new FormFieldPropsOption
                {
                    Label = s.Key,
                    Value = s.Value
                });
            }
        }

        return moduleConfig;
    }
}
