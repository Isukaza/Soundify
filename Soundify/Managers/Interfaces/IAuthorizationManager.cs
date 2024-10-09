using System.Security.Claims;
using Soundify.DAL.PostgreSQL.Roles;

namespace Soundify.Managers.Interfaces;

public interface IAuthorizationManager
{
    string ValidateUserIdentity(List<Claim> claims,
        Guid userId,
        UserRole? compareRole = null,
        Func<UserRole, UserRole, bool> comparison = null);
}