using DentalOffice_BE.Common;
using DentalOffice_BE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalOffice_BE.Services.Models;

public class Section
{
    public long? SectionId { get; set; }
    public string Title { get; set; } = null!;
    public string Route { get; set; } = null!;
    public string? ApiString { get; set; }
    public SectionConfiguration? Configuration { get; set; }
}

public class SectionViewModel
{

    public long Id { get; set; }
    public SectionDto? Section { get; set; }
    public ICollection<SectionDto>? SubSections { get; set; }
    public DateTime? InsertDate { get; set; }
    public DateTime? UpdateDate { get; set; }
}

public class SectionUpdateModel
{
    public long Id { get; set; }
}

public class SectionInsertModel
{
}
