using DentalOffice_BE.Common;
using DentalOffice_BE.Common.Models;
using DentalOffice_BE.Common.Utility;
using DentalOffice_BE.Data;
using DentalOffice_BE.Services.Interfaces;
using DentalOffice_BE.Services.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace DentalOffice_BE.Services.Services;

public class SectionService(DBContext _context) : ISectionService
{
    public async Task<IEnumerable<SectionViewModel>> GetList()
    {
        var query = _context.Sections.AsQueryable();

        query = query.Include(_ => _.SubSections).Where(_ => _.Enabled == true).Include(_ => _.Configuration);

        query = query.Where(_ => _.SectionId == null);

        var data = await query.ToListAsync();

        foreach (var entity in data)
        {
            if(entity.SubSections != null && entity.SubSections.Any())
            {
                entity.SubSections = entity.SubSections.Where(sub => sub.Enabled == true).ToList();
            }
        }

        return data.Select(_ => _.MapViewModelFromDto());
    }

    public async Task<SectionViewModel> GetByRoute(string route)
    {
        var query = _context.Sections.AsQueryable();

        query = query.Include(_ => _.SubSections).Include(_ => _.Configuration);

        query = query.Where(_ => _.Route == '/' + route);

        var data = await query.FirstOrDefaultAsync();

        if (data != null && data.SubSections != null && data.SubSections.Any())
        {
            data.SubSections = data.SubSections.Where(sub => sub.Enabled == true).ToList();
        }

        Validate.ThrowIfNull(data);

        if (data.ApiString is not null && data.ApiString.Contains("lots"))
        {
            string[] parts = data.ApiString.Split("-");
            if (parts.Length > 1)
            {
                long type = long.Parse(parts[1]);
                var materials = await _context.Materials.Where(_ => _.MaterialTypeId == type).OrderByDescending(_ => _.UpdateDate).ToDictionaryAsync(_ => _.Name, _ => _.Id);

                foreach(var material in materials)
                {
                    data?.Configuration?.FormConfiguration?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "materialId")?.Props?.Options?.Add(new FormFieldPropsOption
                    {
                        Label = material.Key, 
                        Value = material.Value
                    });
                }

                if (data?.Configuration?.FormConfiguration?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "colorId") != null)
                {
                    var colors = await _context.Colors.OrderBy(_ => _.UpdateDate).ToDictionaryAsync(_ => _.Code, _ => _.Id);

                    foreach (var c in colors)
                    {
                        data?.Configuration?.FormConfiguration?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "colorId")?.Props?.Options?.Add(new FormFieldPropsOption
                        {
                            Label = c.Key,
                            Value = c.Value
                        });
                    }
                }
            }
        }

        if (data!.ApiString is not null && data.ApiString.Contains("materials"))
        {
            string[] parts = data.ApiString.Split("-");
            if (parts.Length > 1)
            {
                var colors = await _context.Colors.ToListAsync();
                var dentins = await _context.Materials.Include(_ => _.MaterialType).Where(_ => _.MaterialType != null && _.MaterialType.Id == (long)MaterialType.Dentin).ToListAsync();

                foreach (var color in colors)
                {
                    data?.Configuration?.FormConfiguration?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "materialProperties")?.FieldGroup?.FirstOrDefault(_ => _.Key == "color" || _.Key == "colors" || _.Key == "dentinColorsIds")?.Props?.Options?.Add(new FormFieldPropsOption
                    {
                        Label = color.Code,
                        Value = color.Id
                    });
                }

                foreach (var dentin in dentins)
                {
                    data?.Configuration?.FormConfiguration?.FirstOrDefault()?.FieldGroup?.FirstOrDefault(_ => _.Key == "materialProperties")?.FieldGroup?.FirstOrDefault(_ => _.Key == "dentinId")?.Props?.Options?.Add(new FormFieldPropsOption
                    {
                        Label = dentin.Name,
                        Value = dentin.Id
                    });
                }
            }
        }

        

        Validate.ThrowIfNull(data);

        return data.MapViewModelFromDto();
    }

    public async Task<dynamic> GetSingleData(long id, string apiString)
    {
        var query = GetSingleQuery(apiString, id);

        return await query.FirstOrDefaultAsync() ?? null!;
    }

    public async Task<IEnumerable<dynamic>> GetAllData(string apiString)
    {
        var query = GetQuery(apiString);

        var res = await query.ToListAsync();

        foreach (var r in res)
        {
            if (HasProperty(r, "MaterialProperties") && r.MaterialProperties != null)
            {
                var converter = new ExpandoObjectConverter();
                dynamic data = JsonConvert.DeserializeObject<ExpandoObject>(r.MaterialProperties.ToString(), converter);

                if (data != null && HasProperty(data, "dentinId"))
                {
                    long idDaCercare = Convert.ToInt64(((IDictionary<string, object>)data)["dentinId"]);

                    var entity = await _context.Materials.FindAsync(idDaCercare);

                    if (entity != null)
                    {
                        data.name = entity.Name;

                        r.MaterialProperties = data;
                    }
                }
            }
        }

        if (res.Any())
        {
            if (HasProperty(res.First(), "Order"))
            {
                res = res.OrderBy(r => r.Order).ToList();
            }
        }

        return res;
    }

    private IQueryable<dynamic> GetSingleQuery(string apiString, long id)
    {
        if (apiString.Contains("materials"))
        {
            string[] parts = apiString.Split("-");
            if (parts.Length > 1)
            {
                long type = long.Parse(parts[1]);
                return _context.Materials.AsQueryable().Where(_ => _.MaterialTypeId == type).Where(_ => _.Id == id);
            }
        }

        if (apiString.Contains("lots"))
        {
            string[] parts = apiString.Split("-");
            if (parts.Length > 1)
            {
                long type = long.Parse(parts[1]);
                return _context.Lots.AsQueryable().Include(_ => _.Material).Where(_ => _.Material != null && _.Material.MaterialTypeId == type).Where(_ => _.Id == id);
            }
        }

        IQueryable<dynamic> query = apiString switch
        {
            "studios" => _context.Studios.AsQueryable().Where(_ => _.Id == id),
            "colors" => _context.Colors.AsQueryable().Where(_ => _.Id == id),
            "semiproducts" => _context.SemiProducts.AsQueryable().Where(_ => _.Id == id),
            "risks" => _context.Risks.AsQueryable().Where(_ => _.Id == id),
            "stages" => _context.Stages.AsQueryable().Where(_ => _.Id == id),
            "modules" => _context.Modules.AsQueryable().Where(_ => _.Id == id),
            "document_configurations" => _context.DocumentConfigurations.AsQueryable().Where(_ => _.Id == id),
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
                return _context.Lots.AsQueryable().Include(_ => _.Material).Include(_ => _.Color).Where(_ => _.Material != null && _.Material.MaterialTypeId == type);
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
            "document_configurations" => _context.DocumentConfigurations.AsQueryable(),
            _ => throw new Exception("ApiString not allowed")
        };

        return query;
    }

    public async Task InsertData(string apiString, object data)
    {
        var type = apiString.Contains("-") ? apiString.Split("-")[0] : apiString;
        long id = apiString.Contains("-") ? long.Parse(apiString.Split("-")[1]) : 0;

        switch (type)
        {
            case "studios":
                StudioDto? studio = JsonConvert.DeserializeObject<StudioDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(studio);
                _context.Studios.Add(studio);
                break;
            case "colors":
                ColorDto? color = JsonConvert.DeserializeObject<ColorDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(color);
                _context.Colors.Add(color);
                break;
            case "semiproducts":
                SemiProductDto? semiproduct = JsonConvert.DeserializeObject<SemiProductDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(semiproduct);
                _context.SemiProducts.Add(semiproduct);
                break;
            case "risks":
                RiskDto? risk = JsonConvert.DeserializeObject<RiskDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(risk);
                _context.Risks.Add(risk);
                break;
            case "stages":
                StageDto? stage = JsonConvert.DeserializeObject<StageDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(stage);
                _context.Stages.Add(stage);
                break;
            case "modules":
                ModuleDto? module = JsonConvert.DeserializeObject<ModuleDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(module);
                _context.Modules.Add(module);
                break;
            case "document_configurations":
                DocumentConfigurationDto? document_configuration = JsonConvert.DeserializeObject<DocumentConfigurationDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(document_configuration);
                _context.DocumentConfigurations.Add(document_configuration);
                break;
            case "lots":
                LotDto? lot = JsonConvert.DeserializeObject<LotDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(lot);
                _context.Lots.Add(lot);
                break;
            case "materials":
                MaterialDto? materialDto = JsonConvert.DeserializeObject<MaterialDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(materialDto);
                if(materialDto.MaterialProperties != null)
                {
                    materialDto.MaterialProperties = JsonConvert.DeserializeObject<ExpandoObject>(
                        materialDto.MaterialProperties.ToString(),
                        new ExpandoObjectConverter()
                    );
                }
                materialDto.MaterialTypeId = id;
                _context.Materials.Add(materialDto);
                break;
            default: throw new Exception("Route not allowed");
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdateData(string apiString, long id, object data)
    {
        var type = apiString.Contains("-") ? apiString.Split("-")[0] : apiString;

        if (id < 1)
        {
            throw new Exception("Id is mandatory on update");
        }

        switch (type)
        {
            case "studios":
                StudioDto? studio = await _context.Studios.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(studio);
                var studioModel = JsonConvert.DeserializeObject<StudioDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(studioModel);
                studio.Name = studioModel.Name;
                studio.Color = studioModel.Color;
                break;
            case "colors":
                ColorDto? color = await _context.Colors.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(color);
                var colorModel = JsonConvert.DeserializeObject<ColorDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(colorModel);
                color.Code = colorModel.Code;
                break;
            case "semiproducts":
                SemiProductDto? semiproduct = await _context.SemiProducts.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(semiproduct);
                var semiproductModel =  JsonConvert.DeserializeObject<SemiProductDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(semiproductModel);
                semiproduct.Name = semiproductModel.Name;
                break;
            case "risks":
                RiskDto? risk = await _context.Risks.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(risk);
                var riskModel = JsonConvert.DeserializeObject<RiskDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(riskModel);
                risk.Description = riskModel.Description;
                break;
            case "stages":
                StageDto? stage = await _context.Stages.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(stage);
                var stageModel = JsonConvert.DeserializeObject<StageDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(stageModel);
                stage.Name = stageModel.Name;
                break;
            case "modules":
                ModuleDto? module = await _context.Modules.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(module);
                //TODO : PERCHE L'HO MESSO QUA?
                break;
            case "document_configurations":
                DocumentConfigurationDto? documentconfiguration = await _context.DocumentConfigurations.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(documentconfiguration);
                var documentConfigurationModel = JsonConvert.DeserializeObject<DocumentConfigurationDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(documentConfigurationModel);
                documentconfiguration.Name = documentConfigurationModel.Name;
                documentconfiguration.Content = documentConfigurationModel.Content;
                documentconfiguration.CopyCount = documentConfigurationModel.CopyCount;
                documentconfiguration.Order = documentConfigurationModel.Order;
                break;
            case "lots":
                LotDto? lot = await _context.Lots.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(lot);
                var lotModel = JsonConvert.DeserializeObject<LotDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(lotModel);
                lot.Code = lotModel.Code;
                lot.MaterialId = lotModel.MaterialId;
                break;
            case "materials":
                MaterialDto? material = await _context.Materials.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(material);
                var materialModel = JsonConvert.DeserializeObject<MaterialDto>(data.ToString().ThrowIfNull());
                Validate.ThrowIfNull(materialModel);
                if (materialModel.MaterialProperties != null)
                {
                    if (materialModel.MaterialTypeId == (long)MaterialType.Enamel)
                    {
                        material.MaterialProperties = JsonConvert.DeserializeObject<EnamelProperties>(materialModel.MaterialProperties.ToString());
                    }
                }
                material.Name = materialModel.Name;
                break;
            default: throw new Exception("Route not allowed");
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteData(string apiString, long id)
    {
        var type = apiString.Contains("-") ? apiString.Split("-")[0] : apiString;

        switch (type)
        {
            case "studios":
                StudioDto? studio = await _context.Studios.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(studio);
                _context.Remove(studio);
                break;
            case "colors":
                ColorDto? color = await _context.Colors.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(color);
                _context.Remove(color);
                break;
            case "semiproducts":
                SemiProductDto? semiproduct = await _context.SemiProducts.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(semiproduct);
                _context.Remove(semiproduct);
                break;
            case "risks":
                RiskDto? risk = await _context.Risks.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(risk);
                _context.Remove(risk);
                break;
            case "stages":
                StageDto? stage = await _context.Stages.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(stage);
                _context.Remove(stage);
                break;
            case "modules":
                ModuleDto? module = await _context.Modules.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(module);
                //TODO : PERCHE L'HO MESSO QUA?
                break;
            case "document_configurations":
                DocumentConfigurationDto? documentConfiguration = await _context.DocumentConfigurations.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(documentConfiguration);
                _context.Remove(documentConfiguration);
                break;
            case "lots":
                LotDto? lot = await _context.Lots.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(lot);
                _context.Remove(lot);
                break;
            case "materials":
                MaterialDto? material = await _context.Materials.Where(_ => _.Id == id).FirstOrDefaultAsync();
                Validate.ThrowIfNull(material);
                _context.Remove(material);
                break;
            default: throw new Exception("Route not allowed");
        }

        await _context.SaveChangesAsync();
    }
    public static bool HasProperty(dynamic obj, string name)
    {
        if (obj is ExpandoObject)
            return ((IDictionary<string, object>)obj).ContainsKey(name);

        if (obj is Newtonsoft.Json.Linq.JObject)
            return obj[name] != null;

        return obj.GetType().GetProperty(name) != null;
    }
}
