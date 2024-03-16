using DentalOffice_BE.Services.Models;

namespace DentalOffice_BE.Services.Interfaces;

public interface ISectionService
{
    Task<IEnumerable<SectionViewModel>> GetList();
    Task<SectionViewModel> GetByRoute(string route);
}
