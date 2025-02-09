using CatRescueApi.Models;
namespace CatRescueApi.Services
{
    public interface IAdoptionService
    {
        Task<Result<Adoption>> SubmitAdoption(Adoption adoption);
        Task<Adoption?> GetAdoptionById(int id);
        Task<Result<bool>> UpdateStatus(int id, string newStatus); // New method for status updates
        Task<List<Adoption>> GetAllAdoptions();
    }
}