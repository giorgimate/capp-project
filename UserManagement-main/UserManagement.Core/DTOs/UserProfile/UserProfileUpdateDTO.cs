﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.DTOs.UserProfile
{
	public class UserProfileUpdateDTO
	{
        public int Id { get; set; }
        public string FirstName { get; set; }
		public string LastName { get; set; }

		[MaxLength(11)]
		public string PersonalNumber { get; set; }
	}
}
