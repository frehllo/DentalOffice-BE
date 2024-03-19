using DentalOffice_BE.Data;
using DentalOffice_BE.Services.Models;

namespace DentalOffice_BE.Services.Interfaces;

public interface ISectionService
{
    Task<IEnumerable<SectionViewModel>> GetList();
    Task<SectionViewModel> GetByRoute(string apiString);
    Task<IEnumerable<dynamic>> GetAllData(string apiString);
    Task<dynamic> GetSingleData(string id, string apiString);
    Task InsertData(string apiString, object data);
}
