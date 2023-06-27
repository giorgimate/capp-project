using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.DTOs.UserProfile;
using UserManagement.Domain.Models;

namespace UserManagement.Core.Interfaces
{
	public interface IUserProfileService
	{
		Task<ServiceResponse<List<UserProfileGetDTO>>> GetUserProfiles();
		Task<ServiceResponse<UserProfileGetDTO>> GetUserProfileById(int id);
		Task<ServiceResponse<List<UserProfileGetDTO>>> AddUserProfile(UserProfileAddDTO newUserProfile);
		Task<ServiceResponse<UserProfileGetDTO>> UpdateUserProfile(UserProfileUpdateDTO updateUserProfile);
		Task<ServiceResponse<List<UserProfileGetDTO>>> DeleteUserProfile(int id);
	}
}
