using System.Text.Json.Serialization;

namespace CatRescueApi.Models
{
    public class User
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public PersonalDetails Details { get; set; } = new PersonalDetails();
        public Address Location { get; set; } = new Address();
        public bool IsVerified { get; set; } = false;
        public string? VerificationToken { get; set; }
        public DateTime? TokenExpiresAt { get; set; }
        public string Password { get; set; } = string.Empty;
    }

    public class PersonalDetails
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class Address
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}