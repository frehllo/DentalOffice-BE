using DentalOffice_BE.Data;
using DentalOffice_BE.Services.Models;

namespace DentalOffice_BE.Services.Interfaces;

public interface ISectionService
{
    Task<IEnumerable<SectionViewModel>> GetList();
    Task<SectionViewModel> GetByRoute(string route);
    Task<IEnumerable<dynamic>> GetAllData(string apiString);
    Task<dynamic> GetSingleData(long id, string apiString);
    Task InsertData(string apiString, object data);
    Task UpdateData(string apiString, long id, object data);
    Task DeleteData(string apiString, long id);
}
