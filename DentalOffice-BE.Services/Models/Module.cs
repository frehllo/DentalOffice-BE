using DentalOffice_BE.Data;

namespace DentalOffice_BE.Services.Models;

public class Module
{
    public string CustomerName { get; set; } = null!;
    public DateTime PrescriptionDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public string? Description { get; set; }
    public long StudioId { get; set; }
}

public class ModuleListFilter
{
    public string? Filter { get; set; }
    public int? PageIndex { get; set; }
    public int PerPage { get; set; } = 20;
}

public class ModuleListModel { 

    public ModuleListModel(List<ModuleDto> modules, int pageIndex) 
    { 
        this.Modules = modules;
        this.PageIndex = pageIndex;
    }

    public ICollection<ModuleDto> Modules { get; set; }
    public int? PageIndex { get; set; }
}
