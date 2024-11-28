using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPI_4.Abstractions;
using WebAPI_4.DB;
using WebAPI_4.Dto;
using WebAPI_4.Models;

namespace WebAPI_4.Repositories;

public class UserRepository : IUserRepository
{
	private readonly IConfiguration _config;

	public UserRepository(IConfiguration config)
	{  _config = config; }

	public int AddUser(UserDto user)
	{
		using (var context = new UserContext())
		{
			if (context.Users.Any(x => x.Name == user.Name))
				throw new Exception("User already exists.");
			if (user.Role == UserRoleDto.Admin)
				if (context.Users.Any(x => x.RoleId == RoleId.Admin))
					throw new Exception("Admin already exists.");

			var entity = new User { Name = user.Name };

			entity.Salt = new byte[16];
			new Random().NextBytes(entity.Salt);
			var data = Encoding.UTF8.GetBytes(user.Password).Concat(entity.Salt).ToArray();

			SHA512 sha512 = SHA512.Create();
			entity.Password = sha512.ComputeHash(data);
			//entity.Password = new SHA512Managed().ComputeHash(data);
			entity.RoleId=(RoleId)Enum.Parse(typeof(RoleId),user.Role.ToString());
			
			context.Users.Add(entity);
			context.SaveChanges();

			return entity.Id;
		}
	}

	public RoleId CheckUser(LoginDto login)
	{
		using (var context =new UserContext())
		{
			var user = context.Users.FirstOrDefault(x => x.Name == login.Name);

			if (user == null) throw new Exception("No user like this.");

			var data = Encoding.UTF8.GetBytes(login.Password).Concat(user.Salt).ToArray();
			SHA512 sha512 = SHA512.Create();
			var hash=sha512.ComputeHash(data);
			// var hash=new SHA512Managed().ComputeHash(data);

			if (user.Password == hash)
				return user.RoleId;
			
			throw new Exception("Wrong password.");
		}
	}

	public string GenerateToken(UserDto user)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
		var credentials=new SigningCredentials(key,SecurityAlgorithms.Sha512);

		var claim = new[]
		{
			new Claim(ClaimTypes.NameIdentifier, user.Name),
			new Claim(ClaimTypes.Role, user.Role.ToString())
		};

		var token = new JwtSecurityToken(_config["Jwt.Issuer"],
				_config["Jwt.Audience"],
				claim,
				expires: DateTime.UtcNow.AddMinutes(60),
				signingCredentials: credentials);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	//public User Authenticate(UserDto user)
	//{
		
	//	return userNew;
	//}
}
