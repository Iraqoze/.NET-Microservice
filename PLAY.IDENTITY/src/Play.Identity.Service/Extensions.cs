using Play.Identity.Entities;

namespace Play.Identity.Service
{
    public static class Extensions
    {

        public static UserDto AsDto(this User user)
        {
            return new UserDto(user.Firstname, user.Lastname, user.PhoneNumber, user.Email);
        }
    }

}