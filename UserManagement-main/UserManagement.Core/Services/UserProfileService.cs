using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.DTOs.UserProfile;
using UserManagement.Core.Interfaces;
using UserManagement.Domain.Models;
using UserManagement.Domain.Models.Domain;
using UserManagement.Facade.Helpers.Interfaces;

namespace UserManagement.Core.Services
{
    public class UserProfileService : IUserProfileService
	{
		private readonly IMapper _mapper;
		private readonly ApplicationDbContext _db;
		private readonly IHelperMethods _helperMethods;

		public UserProfileService(IMapper mapper, ApplicationDbContext db, IHelperMethods helperMethods)
		{
			_mapper = mapper;
			_db = db;
			_helperMethods = helperMethods;
		}

		public async Task<ServiceResponse<List<UserProfileGetDTO>>> AddUserProfile(UserProfileAddDTO newUserProfile)
		{
			var serviceResponse = new ServiceResponse<List<UserProfileGetDTO>>();
			var userProfile = _mapper.Map<UserProfile>(newUserProfile);

			userProfile.User = await _db.users.FirstOrDefaultAsync(u => u.Id == _helperMethods.GetUserId());

			await _db.userProfiles.AddAsync(userProfile);
			await _db.SaveChangesAsync();
			serviceResponse.Data = await _db.userProfiles
				.Include(c => c.User)
				.Where(c => c.Id == _helperMethods.GetUserId())
				.Select(x => _mapper.Map<UserProfileGetDTO>(x)).ToListAsync();
			return serviceResponse;
		}

		public async Task<ServiceResponse<List<UserProfileGetDTO>>> DeleteUserProfile(int id)
		{
			var serviceResponse = new ServiceResponse<List<UserProfileGetDTO>>();
			try
			{
				var userProfile = await _db.userProfiles
					.FirstOrDefaultAsync(x => x.Id == id && x.User.Id == _helperMethods.GetUserId());
				if (userProfile != null)
				{
					_db.userProfiles.Remove(userProfile);
					await _db.SaveChangesAsync();
					serviceResponse.Data = _db.userProfiles
						.Include(c => c.User)
						.Where(c => c.User.Id == _helperMethods.GetUserId())
						.Select(x => _mapper.Map<UserProfileGetDTO>(x)).ToList();
				}
				else
				{
					serviceResponse.Success = false;
					serviceResponse.Message = "UserProfile not found";
				}
			}
			catch (Exception ex)
			{
				serviceResponse.Success = false;
				serviceResponse.Message = ex.Message;
			}
			return serviceResponse;
		}

		public async Task<ServiceResponse<UserProfileGetDTO>> GetUserProfileById(int id)
		{
			var response = new ServiceResponse<UserProfileGetDTO>();
			var userProfile = await _db.userProfiles
				.Include(u => u.User)
				.FirstOrDefaultAsync(u => u.Id == id && u.User.Id == _helperMethods.GetUserId());
			response.Data = _mapper.Map<UserProfileGetDTO>(userProfile);
			return response;
		}

		public async Task<ServiceResponse<List<UserProfileGetDTO>>> GetUserProfiles()
		{
			var response = new ServiceResponse<List<UserProfileGetDTO>>();
			var userProfiles = await _db.userProfiles
				.Include(u => u.User)
				.Where(u => u.User.Id == _helperMethods.GetUserId()).ToListAsync();
			response.Data = userProfiles.Select(x => _mapper.Map<UserProfileGetDTO>(x)).ToList();
			return response;
		}

		public async Task<ServiceResponse<UserProfileGetDTO>> UpdateUserProfile(UserProfileUpdateDTO updateUserProfile)
		{
			ServiceResponse<UserProfileGetDTO> response = new ServiceResponse<UserProfileGetDTO>();
			try
			{
				var userProfile = await _db.userProfiles
					.Include(u => u.User)
					.FirstOrDefaultAsync(u => u.Id == updateUserProfile.Id);

				if (userProfile.User.Id == _helperMethods.GetUserId())
				{
					_mapper.Map(updateUserProfile, userProfile);
					await _db.SaveChangesAsync();
					response.Data = _mapper.Map<UserProfileGetDTO>(userProfile);
				}
				else
				{
					response.Success = false;
					response.Message = "UserProfile not found";
				}
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
