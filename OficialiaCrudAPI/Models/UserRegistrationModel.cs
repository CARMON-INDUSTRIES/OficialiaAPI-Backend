namespace OficialiaCrudAPI.Models
{
    public class UserRegistrationModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; } = "0000000000";

        public string Password { get; set; }
    }
}
