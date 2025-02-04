namespace CatRescueApi.Models
{
    // Breed model
    public class Breed
    {
        public int Id { get; set; }
        public string Name {get;set;}
        public bool IsGoodWithKids {get;set;}
        public bool IsGoodWithDogs {get; set;}
        public string Description {get;set;} 
    }
}