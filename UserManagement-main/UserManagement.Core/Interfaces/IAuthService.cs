using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Models;
using UserManagement.Domain.Models.Domain;

namespace UserManagement.Core.Interfaces
{
	public interface IAuthService
	{
		Task<ServiceResponse<int>> Register(User user, string password);
		Task<ServiceResponse<string>> Login(string userName, string password);
		Task<bool> UserExists(string userName);
	}
}
