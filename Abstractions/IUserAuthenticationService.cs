using WebAPI_4.Dto;

namespace WebAPI_4.Abstractions;

public interface IUserAuthenticationService
{
	UserDto Authenticate(UserDto user);
}
