using DentalOffice_BE.Data;
using DentalOffice_BE.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalOffice_BE.Services.Interfaces;

public interface IModuleService
{
    Task<ModuleFormConfiguration> GetConfiguration();
    Task<IEnumerable<ModuleDto>> GetList();
    Task<ModuleDto> Get(long id);
    Task<ModuleDto> Insert(ModuleDto model);
    Task<ModuleDto> Update(long id, ModuleDto model);
}
