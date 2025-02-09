using CatRescueApi.Models;
namespace CatRescueApi.Services
{
    public interface IAdoptionService
    {
        Task<Result<Adoption>> SubmitAdoption(Adoption adoption);
        Task<Adoption?> GetAdoptionById(int id);
    }
}