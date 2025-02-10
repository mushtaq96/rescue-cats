using Newtonsoft.Json.Linq;
namespace CatRescueApi.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public PersonalDetailsDto Details { get; set; } = new PersonalDetailsDto();
        public AddressDto Location { get; set; } = new AddressDto();
        public bool IsVerified { get; set; }


        public static UserDto MapToDto(User user)
        {
            var usersJson = File.ReadAllText("./Data/users.json");
            var usersData = JObject.Parse(usersJson);
            var users = usersData["users"]?.ToObject<List<User>>();
            var loggedInUser = users?.FirstOrDefault(u => u.Email == user.Email);

            return new UserDto
            {
                Id = loggedInUser?.Id ?? 0,
                Email = loggedInUser?.Email ?? "",
                Details = new PersonalDetailsDto
                {
                    FirstName = loggedInUser?.Details?.FirstName ?? "",
                    LastName = loggedInUser?.Details?.LastName ?? ""
                },
                Location = new AddressDto
                {
                    Street = loggedInUser?.Location?.Street ?? "",
                    City = loggedInUser?.Location?.City ?? "",
                    State = loggedInUser?.Location?.State ?? "",
                    PostalCode = loggedInUser?.Location?.PostalCode ?? "",
                    Latitude = loggedInUser?.Location?.Latitude ?? 0,
                    Longitude = loggedInUser?.Location?.Longitude ?? 0
                }

            };
        }
    }

    public class PersonalDetailsDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class AddressDto
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}