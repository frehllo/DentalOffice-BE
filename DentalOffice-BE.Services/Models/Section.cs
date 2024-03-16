using DentalOffice_BE.Common;
using DentalOffice_BE.Data;
using Riok.Mapperly.Abstractions;

namespace DentalOffice_BE.Services.Models;

public class Section
{
    public long? SectionId { get; set; }
    public string Title { get; set; } = null!;
    public string Route { get; set; } = null!;
    public string? ApiString { get; set; }
    public ICollection<SectionViewModel>? SubSections { get; set; }
    public SectionConfiguration? Configuration { get; set; } = null;
}

public class SectionViewModel : Section
{

    public long Id { get; set; }
    public DateTime? InsertDate { get; set; }
    public DateTime? UpdateDate { get; set; }
}

public class SectionUpdateModel : Section
{
    public long Id { get; set; }
}

public class SectionInsertModel : Section
{
}

[Mapper]
public static partial class SectionViewModelMapper
{
    public static partial SectionViewModel MapViewModelFromDto(this SectionDto entity);
}

