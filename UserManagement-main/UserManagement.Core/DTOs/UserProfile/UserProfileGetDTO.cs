using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.DTOs.User;

namespace UserManagement.Core.DTOs.UserProfile
{
	public class UserProfileGetDTO
	{
        public int Id { get; set; }
        public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PersonalNumber { get; set; }
		public int UserId { get; set; }
		public UserGetDTO User { get; set; } 
	}
}
