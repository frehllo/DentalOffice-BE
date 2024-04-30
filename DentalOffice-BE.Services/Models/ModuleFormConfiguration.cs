using DentalOffice_BE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DentalOffice_BE.Services.Models;

public class ModuleFormConfiguration
{
    public ICollection<FormFieldConfiguration> PersonalDataForm { get; set; } = null!;
    public ICollection<FormFieldConfiguration> ProcessesForm { get; set; } = null!;
    public ICollection<TableHeaderField> Grid { get; set; } = null!;
}
