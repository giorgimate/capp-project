using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UserManagement.Core.DTOs.User;
using UserManagement.Core.Interfaces;
using UserManagement.Domain.Models;
using UserManagement.Domain.Models.Domain;

namespace UserManagement.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
		{
			var response = await _authService.Register(
				new User { UserName = request.UserName }, request.Password);
			if (!response.Success)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}

		[HttpPost("login")]
		public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request)
		{
			var response = await _authService.Login(request.UserName, request.Password);
			if (!response.Success)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}
	}
}
