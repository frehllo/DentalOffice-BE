using DentalOffice_BE.Common;
using DentalOffice_BE.Common.Models;
using DentalOffice_BE.Common.Utility;
using DentalOffice_BE.Data;
using DentalOffice_BE.Services.Extensions;
using DentalOffice_BE.Services.Interfaces;
using DentalOffice_BE.Services.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DentalOffice_BE.Services.Services;

public class ModuleService(DBContext _context) : IModuleService
{
    public async Task<IEnumerable<ModuleDto>> GetList()
    {
        return await _context.Modules.Include(_ => _.Studio).Include(_ => _.Processes).OrderByDescending(_ => _.UpdateDate).ToListAsync();
    }

    public async Task<ModuleDto> Get(long id)
    {
        var entityDB = await _context.Modules.Include(_ => _.Studio).Where(_ => _.Id == id).FirstOrDefaultAsync();
        
        Validate.ThrowIfNull(entityDB); 
        
        var processes = await _context.Processes.Include(_ => _.Color).Include(_ => _.DentinMaterial).Include(_ => _.MetalMaterial).Where(_ => _.ModuleId == id).ToListAsync();
        entityDB.Processes = processes;

        var stages = await _context.Stages.ToListAsync();

        if (entityDB.Processes != null)
        {
            foreach (var p in entityDB.Processes)
            {
                p.Module = null;

                if (p.StagesIds != null && p.StagesIds.Count() > 0)
                {
                    if(p.Stages == null)
                    {
                        p.Stages = new List<StageDto>();
                    }

                    var procecessStages = stages.Where(_ => p.StagesIds.Contains(_.Id)).ToList();
                    p.Stages = procecessStages;
                }
            }
        }

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
        var entityDB = await _context.Modules.Where(_ => _.Id == id).FirstOrDefaultAsync();
        Validate.ThrowIfNull(entityDB);

        entityDB.CustomerName = model.CustomerName;
        entityDB.PrescriptionDate = model.PrescriptionDate;
        entityDB.DeliveryDate = model.DeliveryDate;
        entityDB.Description = model.Description;
        entityDB.StudioId = model.StudioId;
        
        await _context.SaveChangesAsync();

        var processes = await _context.Processes.Include(_ => _.Color).Include(_ => _.DentinMaterial).Include(_ => _.MetalMaterial).Where(_ => _.ModuleId == id).ToListAsync();
        entityDB.Processes = processes;

        foreach (var process in processes)
        {
            process.Module = null;
        }

        return entityDB;
    }

    public async Task Delete(long id)
    {
        var entityDB = await _context.Modules.Where(_ => _.Id == id).FirstOrDefaultAsync();
        Validate.ThrowIfNull(entityDB);

        _context.Remove(entityDB);
        await _context.SaveChangesAsync();
    }

    public async Task<ModuleFormConfiguration> GetConfiguration()
    {
        //TODO DA CAPIRE LE LAVORAZIONI!!!

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
                moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "semiProductId")?.Props?.Options?.Add(new FormFieldPropsOption
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
                moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "stagesIds")?.Props?.Options?.Add(new FormFieldPropsOption
                {
                    Label = s.Key,
                    Value = s.Value
                });
            }
        }

        return moduleConfig;
    }

    public async Task<KeyValuePair<IEnumerable<FormFieldPropsOption>,IEnumerable<LotDto>>> GetLotsByMaterialId(long materialId)
    {
        var lots = await _context.Lots.Where(_ => _.MaterialId == materialId).Include(_ => _.Material).OrderByDescending(_ => _.UpdateDate).ToListAsync();

        List<FormFieldPropsOption> options = new List<FormFieldPropsOption>();

        if (lots is not null && lots.Count > 0)
        {
            foreach(var lot in lots)
            {
                if (lot.Material is not null && lot.Material.MaterialProperties != null)
                {
                    if (lot.Material!.MaterialTypeId == (long)MaterialType.Enamel)
                    {
                        lot.Material!.MaterialProperties = JsonConvert.DeserializeObject<EnamelProperties>(lot.Material!.MaterialProperties!.ToString());
                    }
                }

                options.Add(new FormFieldPropsOption()
                {
                    Label = lot.Code,
                    Value = lot.Id
                });
            }
        }

        return new KeyValuePair<IEnumerable<FormFieldPropsOption>, IEnumerable<LotDto>>(options, lots!);
    }

    public async Task<object> GetLotsByMaterialIdAndColorId(long materialId, long colorId)
    {
        var dentinLots = await _context.Lots.Where(_ => _.MaterialId == materialId).Include(_ => _.Material).OrderByDescending(_ => _.UpdateDate).ToListAsync();
        dentinLots = dentinLots.Where(lot => lot.ColorId is not null && lot.ColorId == colorId).ToList();

        var enamelLots = await _context.Lots.Include(_ => _.Material).Where(_ => _.Material!.MaterialTypeId == (long)MaterialType.Enamel).OrderByDescending(_ => _.UpdateDate).ToListAsync();

        List<FormFieldPropsOption> enamelLotsOptions = new List<FormFieldPropsOption>();

        foreach (var el in enamelLots)
        {
            EnamelProperties props = JsonConvert.DeserializeObject<EnamelProperties>(el.Material!.MaterialProperties!.ToString());
            if (props.dentinColorsIds is not null && props.dentinColorsIds.Count() > 0 && props.dentinColorsIds.Contains(colorId))
            {
                enamelLotsOptions.Add(new FormFieldPropsOption() { Label = el.Code, Value = el.Id });

            }
        }

        List<FormFieldPropsOption> dentinLotsOptions = new List<FormFieldPropsOption>();

        foreach(var dl in dentinLots)
        {
            dentinLotsOptions.Add(new FormFieldPropsOption() { Label = dl.Code, Value = dl.Id });
        }

        return new { dentinLots = dentinLotsOptions, enamelLots = enamelLotsOptions };
    }

    public async Task<IEnumerable<DocumentConfigurationDto>> GetDocumentsPrintPreviews(long id)
    {
        var entitiesDB = await _context.DocumentConfigurations.OrderBy(_ => _.Order).ToListAsync();
        Validate.ThrowIfNull(entitiesDB);
        var moduleDB = await _context.Modules.Include(_ => _.Studio).Where(_ => _.Id == id)
            //.Include(_ => _.Processes)
            //.Include(_ => _.Processes)!.ThenInclude(_ => _.MetalMaterial)
            //.Include(_ => _.Processes)!.ThenInclude(_ => _.DentinMaterial)
            //.Include(_ => _.Processes)!.ThenInclude(_ => _.Color)
            .FirstOrDefaultAsync();
        Validate.ThrowIfNull(moduleDB);

        var processesDB = await _context.Processes.Include(_ => _.Color).Include(_ => _.DentinMaterial).Include(_ => _.MetalMaterial).Include(_ => _.SemiProduct).Include(_ => _.MetalLot).Include(_ => _.DentinLot).Include(_ => _.EnamelLot).Where(_ => _.ModuleId == id).ToListAsync();
        var stagesDB = await _context.Stages.ToListAsync();
        if(stagesDB.Count > 0)
        {
            foreach (var process in processesDB)
            {
                if (process.StagesIds != null && process.StagesIds.Count() > 0)
                {
                    if (process.Stages == null)
                    {
                        process.Stages = new List<StageDto>();
                    }

                    var procecessStages = stagesDB.Where(_ => process.StagesIds.Contains(_.Id)).ToList();
                    process.Stages = procecessStages;
                }
            }
        }
        moduleDB.Processes = processesDB;

        if (entitiesDB.Count > 0)
        {
            foreach(var doc in entitiesDB)
            {
                doc.Content = doc.Content.ReplaceAndExecuteCode<ModuleDto>(moduleDB);
            }
        }

        return entitiesDB;
    }

    public async Task<ProcessDto> AddProcess(ProcessDto model)
    {
        _context.Processes.Add(model);
        await _context.SaveChangesAsync();

        var entityDB = await _context.Processes.Where(_ => _.Id == model.Id).Include(_ => _.DentinMaterial).Include(_ => _.MetalMaterial).Include(_ => _.Color).FirstOrDefaultAsync();
        Validate.ThrowIfNull(entityDB);

        return entityDB;
    }

    public async Task<ProcessDto> UpdateProcess(long id, ProcessDto model)
    {
        var entityDB = await _context.Processes.Where(_ => _.Id == id).FirstOrDefaultAsync();
        Validate.ThrowIfNull(entityDB);

        entityDB.MetalMaterialId = model.MetalMaterialId;
        entityDB.MetalLotId = model.MetalLotId;
        entityDB.DentinLotId = model.DentinLotId;
        entityDB.DentinMaterialId = model.DentinMaterialId;
        entityDB.ColorId = model.ColorId;
        entityDB.EnamelLotId = model.EnamelLotId;
        entityDB.RiskId = model.RiskId;
        entityDB.SemiProductId = model.SemiProductId;
        entityDB.StagesIds = model.StagesIds;

        await _context.SaveChangesAsync();

        var result = await _context.Processes.Where(_ => _.Id == model.Id).Include(_ => _.DentinMaterial).Include(_ => _.MetalMaterial).Include(_ => _.Color).FirstOrDefaultAsync();
        Validate.ThrowIfNull(result);

        return result;
    }

    public async Task RemoveProcess(long id)
    {
        var entityDb = _context.Processes.Find(id);
        Validate.ThrowIfNull(entityDb);

        _context.Remove(entityDb);
        await _context.SaveChangesAsync();
    }
}
