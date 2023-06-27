using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UserManagement.Facade.Helpers.Interfaces;

namespace UserManagement.Facade.Helpers.Implementations
{
	public class HelperMethods : IHelperMethods
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HelperMethods(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public int GetUserId()
        {
            return int.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}

