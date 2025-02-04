namespace CatRescueApi.Models
{
    // Cat model
    public class Cat
    {
        public int Id { get; set; }
        public string UserId {get;set;}
        public int CatId {get;set;}
        public string TenantId {get; set;}
    }
}