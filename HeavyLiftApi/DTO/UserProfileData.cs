using HeavyLiftApi.Models;

namespace HeavyLiftApi.DTO
{
    public class UserProfileData
    {
        public int id { get; set; }

        public string email { get; set; } = null!;

        public string nickname { get; set; } = null!;

        public byte[]? profilepicture { get; set; }

        public UserProfileData(user User)
        {
            id = User.id;
            email = User.email;
            nickname = User.nickname;
            profilepicture = User.profilepicture;
        }
    }
}
