namespace CatRescueApi.DTOs
{
    public class AdoptionDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CatId { get; set; }
        public string Status { get; set; }
    }
}