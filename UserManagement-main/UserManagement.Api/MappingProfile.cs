using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.DTOs.User;
using UserManagement.Core.DTOs.UserProfile;
using UserManagement.Domain.Models.Domain;

namespace UserManagement.Core
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<UserProfile, UserProfileAddDTO>().ReverseMap();
            CreateMap<UserProfile, UserProfileGetDTO>().ReverseMap();
            CreateMap<UserProfile, UserProfileUpdateDTO>().ReverseMap();
            CreateMap<User, UserGetDTO>().ReverseMap();
        }
    }
}
