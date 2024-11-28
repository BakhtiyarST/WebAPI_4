using WebAPI_4.Abstractions;
using WebAPI_4.Models;

namespace WebAPI_4.Dto
{
	public class UserAuthenticationService : IUserAuthenticationService
	{
		public UserDto Authenticate(UserDto user)
		{
			if (user.Name == "admin" && user.Password == "bird")
				return new UserDto { Name=user.Name, Password=user.Password, Role=UserRoleDto.Admin};
			if (user.Name == "user" && user.Password == "bittle")
				return new UserDto { Name = user.Name, Password = user.Password, Role = UserRoleDto.User };
			return null;
		}
	}
}
