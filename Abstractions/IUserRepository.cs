using WebAPI_4.Dto;
using WebAPI_4.Models;

namespace WebAPI_4.Abstractions;

public interface IUserRepository
{
	int AddUser(UserDto user);
	RoleId CheckUser(LoginDto login);
	string GenerateToken(UserDto user);
	// User Authenticate(UserDto user);
}
