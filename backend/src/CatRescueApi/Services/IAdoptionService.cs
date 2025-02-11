using CatRescueApi.Models;
namespace CatRescueApi.Services
{
    public interface IAdoptionService
    {
        Task<Result<Adoption>> SubmitAdoption(Adoption adoption);
        Task<Adoption?> GetAdoptionById(string id);
        Task<Result<bool>> UpdateStatus(string id, string newStatus);
        Task<List<Adoption>> GetAllAdoptions();
        Task<bool> CheckIfUserHasApplied(string catId, string userId);
    }
}