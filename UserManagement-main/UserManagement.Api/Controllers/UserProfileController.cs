using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Core.DTOs.UserProfile;
using UserManagement.Core.Interfaces;
using UserManagement.Domain.Models;

namespace UserManagement.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[Authorize]
	public class UserProfileController : ControllerBase
	{
		private readonly IUserProfileService _userProfileService;

		public UserProfileController(IUserProfileService userProfileService)
		{
			_userProfileService = userProfileService;
		}

		[HttpGet]
		public async Task<ActionResult<ServiceResponse<List<UserProfileGetDTO>>>> GetAll()
		{
			return await _userProfileService.GetUserProfiles();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ServiceResponse<UserProfileGetDTO>>> GetSingle(int id)
		{
			return await _userProfileService.GetUserProfileById(id);
		}

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<List<UserProfileGetDTO>>>> CreateUserProfile(UserProfileAddDTO newUserProfile)
		{
			return await _userProfileService.AddUserProfile(newUserProfile);
		}

		[HttpPut]
		public async Task<ActionResult<ServiceResponse<UserProfileGetDTO>>> UpdateUserProfile(UserProfileUpdateDTO updatedUserProfile)
		{
			var response = await _userProfileService.UpdateUserProfile(updatedUserProfile);
			if (response.Data == null)
			{
				return NotFound(response);
			}
			return Ok(response);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<ServiceResponse<List<UserProfileGetDTO>>>> DeleteUserProfile(int id)
		{
			var response = await _userProfileService.DeleteUserProfile(id);
			if (response.Data == null)
			{
				return NotFound(response);
			}
			return Ok(response);
		}
	}
}
