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
    public async Task<ModuleListModel> GetList(ModuleListFilter filters)
    {
        int pageIndex = filters.PageIndex ?? 1;
        int pageSize = filters.PerPage > 0 ? filters.PerPage : 50;
        int skip = (pageIndex - 1) * pageSize;

        var query = _context.Modules
            .Include(m => m.Studio)
            .Include(m => m.Processes)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filters.Filter))
        {
            var term = filters.Filter.Trim().ToLower();
            query = query.Where(m =>
                m.CustomerName.ToLower().Contains(term) ||
                (m.Description != null && m.Description.ToLower().Contains(term)) ||
                (m.Studio != null && m.Studio.Name.ToLower().Contains(term))
            );
        }

        var filteredModules = await query
            .OrderByDescending(m => m.UpdateDate)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        return new ModuleListModel(filteredModules, pageIndex);
    }

    public async Task<ModuleDto> Get(long id)
    {
        var entityDB = await _context.Modules.Include(_ => _.Studio).Where(_ => _.Id == id).FirstOrDefaultAsync();
        
        Validate.ThrowIfNull(entityDB); 
        
        var processes = await _context.Processes.Include(_ => _.Color).Include(_ => _.DentinMaterial).Include(_ => _.MetalMaterial).Include(_ => _.DiskMaterial).Where(_ => _.ModuleId == id).ToListAsync();
        entityDB.Processes = processes;

        var stages = await _context.Stages.ToListAsync();

        if (entityDB.Processes != null)
        {
            foreach (var p in entityDB.Processes)
            {
                p.Module = null;

                p.SemiProduct = p.SemiProductId != null ? await _context.SemiProducts.FindAsync(p.SemiProductId) : null;

                p.Name = p.GetRiepilogo(p.SemiProduct, p.MetalMaterial, p.DentinMaterial, p.DiskMaterial);

                p.SemiProduct = null;

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
        entityDB.PrescriptionDate = model.PrescriptionDate != null ? model.PrescriptionDate.Value.ToUniversalTime() : model.PrescriptionDate;
        entityDB.DeliveryDate = model.DeliveryDate != null ? model.DeliveryDate.Value.ToUniversalTime() : model.DeliveryDate;
        entityDB.Description = model.Description;
        entityDB.StudioId = model.StudioId;
        
        await _context.SaveChangesAsync();

        entityDB.Processes = await GetProcessesList(id);

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

        string baseDirectory = AppContext.BaseDirectory;
        
        string filePath = Path.Combine(baseDirectory, "json", "module.json");

        using (StreamReader r = new StreamReader(filePath))
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
            if(moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup != null)
            {
                AddDefaultSelectOption(moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.ToList());
            }

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

            var metalMaterialLotsGroup = await _context.Lots
                .Include(l => l.Material)
                .Include(l => l.Color)
                .Where(l => l.Material!.MaterialTypeId == (long)MaterialType.Metal)
                .GroupBy(l => new { l.MaterialId, l.ColorId })
                .Select(g => g.OrderByDescending(x => x.UpdateDate).First())
                .ToListAsync();

            var metalMaterialLots = metalMaterialLotsGroup.ToDictionary(
                l => {
                    var materialName = l.Material?.Name ?? "N/A";
                    return $"{l.Code} - {materialName}";
                },
                l => l.Id
            );

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

            var dentinMaterialLotsGroup = await _context.Lots
                .Include(l => l.Material)
                .Include(l => l.Color)
                .Where(l => l.Material!.MaterialTypeId == (long)MaterialType.Dentin)
                .GroupBy(l => new { l.MaterialId, l.ColorId })
                .Select(g => g.OrderByDescending(x => x.UpdateDate).First())
                .ToListAsync();

            var dentinMaterialLots = dentinMaterialLotsGroup.ToDictionary(
                l => {
                    var materialName = l.Material?.Name ?? "N/A";
                    var colorName = l.Color != null ? $" ({l.Color.Code})" : "";
                    return $"{l.Code} - {materialName}{colorName}";
                },
                l => l.Id
            );

            foreach (var d in dentinMaterialLots)
            {
                moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "dentinLotId")?.Props?.Options?.Add(new FormFieldPropsOption
                {
                    Label = d.Key,
                    Value = d.Value
                });
            }

            var enamelLotsDto = await _context.Lots
                .Include(l => l.Material)
                .Where(l => l.Material!.MaterialTypeId == (long)MaterialType.Enamel)
                .OrderByDescending(l => l.UpdateDate)
                .ToListAsync();

            var processedLots = enamelLotsDto
                .Select(l => {
                    long? dId = null;
                    if (l.Material?.MaterialProperties != null)
                    {
                        var props = JsonConvert.DeserializeObject<dynamic>(l.Material.MaterialProperties.ToString());
                        dId = (long?)props?.dentinId;
                    }
                    return new { Lot = l, DentinId = dId };
                })
                .GroupBy(x => new { x.Lot.MaterialId, x.DentinId })
                .Select(g => g.First())
                .ToList();

            var allDentinIds = processedLots
                .Where(x => x.DentinId.HasValue)
                .Select(x => x.DentinId!.Value)
                .Distinct()
                .ToList();

            var dentinNamesMap = await _context.Materials
                .Where(m => allDentinIds.Contains(m.Id))
                .ToDictionaryAsync(m => m.Id, m => m.Name);

            var enamelMaterialLots = processedLots.ToDictionary(
                x => {
                    var lot = x.Lot;
                    var materialName = lot.Material?.Name ?? "N/A";

                    var dentinSuffix = x.DentinId.HasValue && dentinNamesMap.TryGetValue(x.DentinId.Value, out var dName)
                        ? $" - {dName}"
                        : "";

                    return $"{lot.Code} - {materialName}{dentinSuffix}";
                },
                x => x.Lot.Id
            );

            foreach (var m in enamelMaterialLots)
            {
                moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "enamelLotId")?.Props?.Options?.Add(new FormFieldPropsOption
                {
                    Label = m.Key,
                    Value = m.Value
                });
            }

            var diskMaterials = await _context.Materials.OrderByDescending(_ => _.UpdateDate).Where(_ => _.MaterialTypeId == (long)MaterialType.PolycarbonateDisc).ToDictionaryAsync(_ => _.Name, _ => _.Id);

            foreach (var m in diskMaterials)
            {
                moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "diskMaterialId")?.Props?.Options?.Add(new FormFieldPropsOption
                {
                    Label = m.Key,
                    Value = m.Value
                });
            }

            var diskMaterialsLots = await _context.Lots.Include(_ => _.Material).Where(_ => _.Material!.MaterialTypeId == (long)MaterialType.PolycarbonateDisc).OrderByDescending(_ => _.UpdateDate).ToDictionaryAsync(_ => $"{(_.Material != null ? $"{_.Code} - {_.Material.Name}" : _.Code)}", _ => _.Id);

            foreach (var m in diskMaterialsLots)
            {
                moduleConfig.ProcessesForm?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "diskLotId")?.Props?.Options?.Add(new FormFieldPropsOption
                {
                    Label = m.Key,
                    Value = m.Value
                });
            }

            var colors = await _context.Colors.OrderBy(_ => _.UpdateDate).ToDictionaryAsync(_ => _.Code, _ => _.Id);

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

    private void AddDefaultSelectOption(List<FormFieldConfiguration>? fields)
    {
        if (fields == null) return;

        foreach (var field in fields)
        {
            // 1. Se il campo ha delle Options, aggiungi "Seleziona" in testa
            if (field.Props?.Options != null)
            {
                field.Props.Options.Add(new FormFieldPropsOption
                {
                    Label = "-- Seleziona",
                    Value = null
                });
            }

            // 2. Se il campo ha dei sotto-gruppi (FieldGroup), richiama il metodo ricorsivamente
            if (field.FieldGroup != null && field.FieldGroup.Any())
            {
                AddDefaultSelectOption(fields);
            }
        }
    }

    public async Task<KeyValuePair<IEnumerable<FormFieldPropsOption>,IEnumerable<LotDto>>> GetLotsByMaterialId(long materialId)
    {
        var lots = await _context.Lots.Where(_ => _.MaterialId == materialId).Include(_ => _.Material).OrderByDescending(_ => _.UpdateDate).ToListAsync();

        List<FormFieldPropsOption> options = new List<FormFieldPropsOption>();

        options.Add(new FormFieldPropsOption() { Label = "-- Seleziona", Value = null });

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
        var dentin = await _context.Materials.FindAsync(materialId);
        Validate.ThrowIfNull(dentin);

        var dentinLots = await _context.Lots.Where(_ => _.MaterialId == materialId).Include(_ => _.Material).OrderByDescending(_ => _.UpdateDate).ToListAsync();
        dentinLots = dentinLots.Where(lot => lot.ColorId is not null && lot.ColorId == colorId).ToList();

        var enamelLots = await _context.Lots.Include(_ => _.Material).Where(_ => _.Material!.MaterialTypeId == (long)MaterialType.Enamel).OrderByDescending(_ => _.UpdateDate).ToListAsync();

        List<FormFieldPropsOption> enamelLotsOptions = new List<FormFieldPropsOption>();

        enamelLotsOptions.Add(new FormFieldPropsOption() { Label = "-- Seleziona", Value = null });

        foreach (var el in enamelLots)
        {
            EnamelProperties props = JsonConvert.DeserializeObject<EnamelProperties>(el.Material!.MaterialProperties!.ToString());
            if (props.dentinColorsIds is not null && props.dentinColorsIds.Count() > 0 && props.dentinColorsIds.Contains(colorId) && props.dentinId != null && props.dentinId == dentin.Id)
            {
                enamelLotsOptions.Add(new FormFieldPropsOption() { Label = el.Code, Value = el.Id });
            }
        }

        List<FormFieldPropsOption> dentinLotsOptions = new List<FormFieldPropsOption>();

        dentinLotsOptions.Add(new FormFieldPropsOption() { Label = "-- Seleziona", Value = null });

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

    public async Task<IList<ProcessDto>> AddProcess(ProcessDto model)
    {
        _context.Processes.Add(model);

        await UpdateModuleDescription(model);

        await _context.SaveChangesAsync();

        return await GetProcessesList(model.ModuleId);
    }

    private async Task UpdateModuleDescription(ProcessDto process)
    {
        if (process.Dentals == null || !process.Dentals.Any()) return;

        var module = await _context.Modules.FirstOrDefaultAsync(m => m.Id == process.ModuleId);
        if (module == null) return;

        string? dentalList = process.Dentals.Any() ? string.Join(",", process.Dentals) : null;
        string? detail = null;

        if (!string.IsNullOrEmpty(process.ProcessDescription))
        {
            detail = process.ProcessDescription + $" su {dentalList}";
        }else if (process.MetalMaterialId != null || process.DentinMaterialId != null)
        {
            var materialId = process.MetalMaterialId ?? process.DentinMaterialId;
            var materialName = (await _context.Materials.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == materialId))?.Name;

            if (materialName != null)
                detail = $"{process.Dentals.Length} elementi in {materialName} su {dentalList}";
        }

        if (!string.IsNullOrEmpty(detail))
        {
            if (!string.IsNullOrEmpty(module.Description))
            {
                module.Description += $" + {detail}";
            }
            else
            {
                module.Description = detail;
            }
        }
    }

    public async Task<IList<ProcessDto>> UpdateProcess(long id, ProcessDto model)
    {
        var entityDB = await _context.Processes.Where(_ => _.Id == id).FirstOrDefaultAsync();
        Validate.ThrowIfNull(entityDB);

        entityDB.MetalMaterialId = model.MetalMaterialId;
        entityDB.MetalLotId = model.MetalLotId;
        entityDB.DentinLotId = model.DentinLotId;
        entityDB.DentinMaterialId = model.DentinMaterialId;
        entityDB.ColorId = model.ColorId;
        entityDB.EnamelLotId = model.EnamelLotId;
        entityDB.DiskMaterialId = model.DiskMaterialId;
        entityDB.DiskLotId = model.DiskLotId;
        entityDB.RiskId = model.RiskId;
        entityDB.SemiProductId = model.SemiProductId;
        entityDB.StagesIds = model.StagesIds;
        entityDB.Others = model.Others;
        entityDB.MetalCustom = model.MetalCustom;
        entityDB.MetalLotCustom = model.MetalLotCustom;
        entityDB.DentinCustom = model.DentinCustom;
        entityDB.DentinLotCustom = model.DentinLotCustom;
        entityDB.EnamelCustom = model.EnamelCustom;
        entityDB.EnamelLotCustom = model.EnamelLotCustom;
        entityDB.DiskCustom = model.DiskCustom;
        entityDB.DiskLotCustom = model.DiskLotCustom;

        await _context.SaveChangesAsync();

        var result = await _context.Processes.Where(_ => _.Id == model.Id).Include(_ => _.DentinMaterial).Include(_ => _.MetalMaterial).Include(_ => _.Color).FirstOrDefaultAsync();

        Validate.ThrowIfNull(result);

        return await GetProcessesList(model.ModuleId); ;
    }

    private async Task<IList<ProcessDto>> GetProcessesList(long moduleId)
    {
        Validate.ThrowIfNull(moduleId);

        var entitiesDB = await _context.Processes.Include(_ => _.MetalMaterial).Include(_ => _.DentinMaterial).Include(_ => _.DiskMaterial).Where(p => p.ModuleId == moduleId).ToListAsync();

        foreach ( var entityDB in entitiesDB)
        {
            entityDB.Module = null;
            entityDB.Name = entityDB.GetRiepilogo(entityDB.SemiProduct, entityDB.MetalMaterial, entityDB.DentinMaterial, entityDB.DiskMaterial);
        }

        return entitiesDB;
    }

    public async Task RemoveProcess(long id)
    {
        var entityDb = _context.Processes.Find(id);
        Validate.ThrowIfNull(entityDb);

        _context.Remove(entityDb);
        await _context.SaveChangesAsync();
    }
}
