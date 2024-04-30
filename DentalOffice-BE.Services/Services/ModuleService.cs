using DentalOffice_BE.Common;
using DentalOffice_BE.Common.Models;
using DentalOffice_BE.Common.Utility;
using DentalOffice_BE.Data;
using DentalOffice_BE.Services.Interfaces;
using DentalOffice_BE.Services.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DentalOffice_BE.Services.Services;

public class ModuleService(DBContext _context) : IModuleService
{
    public async Task<IEnumerable<ModuleDto>> GetList()
    {
        return await _context.Modules.Include(_ => _.Studio).OrderByDescending(_ => _.UpdateDate).ToListAsync();
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

    public async Task<ModuleDto> Update(long id, ModuleDto model)
    {
        var entityDb = await _context.Modules.Where(_ => _.Id == id).FirstOrDefaultAsync();
        Validate.ThrowIfNull(entityDb);

        entityDb.CustomerName = model.CustomerName;
        entityDb.PrescriptionDate = model.DeliveryDate;
        entityDb.Description = model.Description;
        entityDb.StudioId = model.StudioId;
        entityDb.Processes = model.Processes;

        await _context.SaveChangesAsync();

        return entityDb;
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

        if (moduleConfig.ProcessesForm != null && moduleConfig.ProcessesForm.Count > 0)
        {
            var semiProducts = await _context.SemiProducts.OrderByDescending(_ => _.UpdateDate).ToDictionaryAsync(_ => _.Name, _ => _.Id);

            foreach (var s in semiProducts)
            {
                moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "semiproductId")?.Props?.Options?.Add(new FormFieldPropsOption
                {
                    Label = s.Key,
                    Value = s.Value
                });
            }

            var metalMaterials = await _context.Materials.OrderByDescending(_ => _.UpdateDate).Where(_ => _.MaterialTypeId == (long)MaterialType.Metal).ToDictionaryAsync(_ => _.Name, _ => _.Id);

            foreach (var m in metalMaterials)
            {
                moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "metalMaterialId")?.Props?.Options?.Add(new FormFieldPropsOption
                {
                    Label = m.Key,
                    Value = m.Value
                });
            }

            var metalMaterialLots = await _context.Lots.Include(_ => _.Material).Where(_ => _.Material!.MaterialTypeId == (long)MaterialType.Metal).OrderByDescending(_ => _.UpdateDate).ToDictionaryAsync(_ => _.Code, _ => _.Id);

            foreach (var m in metalMaterialLots)
            {
                moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "metalLotId")?.Props?.Options?.Add(new FormFieldPropsOption
                {
                    Label = m.Key,
                    Value = m.Value
                });
            }

            var dentinMaterials = await _context.Materials.OrderByDescending(_ => _.UpdateDate).Where(_ => _.MaterialTypeId == (long)MaterialType.Dentin).ToDictionaryAsync(_ => _.Name, _ => _.Id);

            foreach (var d in dentinMaterials)
            {
                moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "dentinMaterialId")?.Props?.Options?.Add(new FormFieldPropsOption
                {
                    Label = d.Key,
                    Value = d.Value
                });
            }

            var dentinMaterialLots = await _context.Lots.Include(_ => _.Material).Where(_ => _.Material!.MaterialTypeId == (long)MaterialType.Dentin).ToDictionaryAsync(_ => _.Code, _ => _.Id);

            foreach (var d in dentinMaterialLots)
            {
                moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "dentinLotId")?.Props?.Options?.Add(new FormFieldPropsOption
                {
                    Label = d.Key,
                    Value = d.Value
                });
            }

            var enamelMaterialLots = await _context.Lots.Include(_ => _.Material).Where(_ => _.Material!.MaterialTypeId == (long)MaterialType.Enamel).OrderByDescending(_ => _.UpdateDate).ToDictionaryAsync(_ => _.Code, _ => _.Id);

            foreach (var m in enamelMaterialLots)
            {
                moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "enamelLotId")?.Props?.Options?.Add(new FormFieldPropsOption
                {
                    Label = m.Key,
                    Value = m.Value
                });
            }

            var colors = await _context.Colors.OrderByDescending(_ => _.UpdateDate).ToDictionaryAsync(_ => _.Code, _ => _.Id);

            foreach (var c in colors)
            {
                moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "colorId")?.Props?.Options?.Add(new FormFieldPropsOption
                {
                    Label = c.Key,
                    Value = c.Value
                });
            }

            var risks = await _context.Risks.OrderByDescending(_ => _.UpdateDate).ToDictionaryAsync(_ => _.Description, _ => _.Id);

            foreach (var r in risks)
            {
                moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "riskId")?.Props?.Options?.Add(new FormFieldPropsOption
                {
                    Label = r.Key,
                    Value = r.Value
                });
            }

            var stages = await _context.Stages.OrderByDescending(_ => _.UpdateDate).ToDictionaryAsync(_ => _.Name, _ => _.Id);

            foreach (var s in stages)
            {
                moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "stageIds")?.Props?.Options?.Add(new FormFieldPropsOption
                {
                    Label = s.Key,
                    Value = s.Value
                });
            }
        }

        return moduleConfig;
    }
}
