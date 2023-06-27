using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Interfaces;
using UserManagement.Domain;
using UserManagement.Domain.Models;
using UserManagement.Domain.Models.Domain;

namespace UserManagement.Core.Services
{
	public class AuthService : IAuthService
	{
		private readonly ApplicationDbContext _db;
		private readonly IConfiguration _configuration;
		public AuthService(ApplicationDbContext db, IConfiguration configuration)
		{
			_db = db;
			_configuration = configuration;
		}

		public async Task<ServiceResponse<string>> Login(string userName, string password)
		{
			var response = new ServiceResponse<string>();
			var user = await _db.users.FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
			if (user == null)
			{
				response.Success = false;
				response.Message = "User not found.";
			}
			else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
			{
				response.Success = false;
				response.Message = "Wrong password.";
			}
			else
			{
				response.Data = GenerateToken(user);
			}

			return response;
		}

		public async Task<ServiceResponse<int>> Register(User user, string password)
		{
			var response = new ServiceResponse<int>();
			if (await UserExists(user.UserName))
			{
				response.Success = false;
				response.Message = "User already exists";
				return response;
			}
			CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
			user.PasswordHash = passwordHash;
			user.PasswordSalt = passwordSalt;
			user.IsActive = true;
			user.Email = user.UserName;

			_db.users.Add(user);
			await _db.SaveChangesAsync();
			response.Data = user.Id;
			return response;
		}

		public async Task<bool> UserExists(string userName)
		{
			if (await _db.users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower()))
			{
				return true;
			}
			return false;
		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}

		private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
			{
				var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				return computeHash.SequenceEqual(passwordHash);
			}
		}

		private string GenerateToken(User user)
		{
			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.UserName)
			};

			SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
				.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

			SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = creds
			};

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}
