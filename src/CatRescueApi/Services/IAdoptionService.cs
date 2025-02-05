using System.Collections.Generic;
using System.Threading.Tasks;
using CatRescueApi.DTOs;
using CatRescueApi.Models;

namespace CatRescueApi.Services
{
    public interface IAdoptionService
    {
        Task<AdoptionDto> SubmitAdoptionAsync(AdoptionRequest request);
        Task<AdoptionDto> GetAdoptionByIdAsync(int id);
    }
}