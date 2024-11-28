using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI_4.Abstractions;
using WebAPI_4.Dto;
using WebAPI_4.Models;

namespace WebAPI_4.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
	private readonly IUserRepository _repository;
	private readonly IConfiguration _config;
	public UserController(IUserRepository repository, IConfiguration config)
	{
		_repository = repository;
		_config = config;
	}

	[HttpPost]
	public ActionResult<int> AddUser(UserDto user)
	{
		try
		{
			return Ok(_repository.AddUser(user));
		}
		catch (Exception ex)
		{
			return StatusCode(409);
		}
	}

	[HttpPost]
	public ActionResult<RoleId> CheckUser(LoginDto login)
	{
		try
		{
			return Ok(_repository.CheckUser(login));
		}
		catch (Exception ex)
		{
			return StatusCode(409);
		}
	}

	//[HttpGet]
	//public ActionResult<string> GenerateToken(UserDto user)
	//{
	//	try
	//	{
	//		return Ok(_repository.GenerateToken(user));
	//	}
	//	catch (Exception ex)
	//	{
	//		return StatusCode(500);
	//	}
	//}
	

	// Homework: возвращение токена
	// Homework: в контроллере будет 3 метода: 2 post метода, 1 private - generate token через RSAExtensions (GetPrivateKey)

}
