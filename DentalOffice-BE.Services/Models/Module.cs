using DentalOffice_BE.Data;
using System.Text.Json.Serialization;

namespace DentalOffice_BE.Services.Models;

public class Module
{
    public string CustomerName { get; set; } = null!;
    public DateTime PrescriptionDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public string? Description { get; set; }
    public long StudioId { get; set; }
}

public class ModuleViewModel : Module
{
    public StudioDto? Studio { get; set; }
    public ICollection<ProcessDto>? Processes { get; set; }
    public ICollection<DocumentInstanceDto>? Instances { get; }
}

public class ModuleInsertModel : Module
{
    public ICollection<ProcessDto>? Processes { get; set; }
}
