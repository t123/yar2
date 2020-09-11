using Yar.Data;

namespace Yar.Api.Models
{
    public class UserIndexModel
    {
        public string Username { get; set; }

        public static UserIndexModel From(User user)
        {
            return new UserIndexModel
            {
                Username = user.Username
            };
        }
    }
}
