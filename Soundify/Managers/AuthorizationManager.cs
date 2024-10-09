using System.Security.Claims;
using Soundify.DAL.PostgreSQL.Roles;
using Soundify.Managers.Interfaces;

namespace Soundify.Managers;

public class AuthorizationManager : IAuthorizationManager
{
    public string ValidateUserIdentity(List<Claim> claims,
        Guid userId,
        UserRole? compareRole = null,
        Func<UserRole, UserRole, bool> comparison = null)
    {
        var userIdFromClaim = claims.GetUserId();
        var role = claims.GetUserRole();
        if (role is null || userIdFromClaim is null)
            return "Authorization failed due to an invalid or missing role in the provided token";

        if (comparison == null)
        {
            if (userIdFromClaim.Value != userId)
                return "You do not have permission to access other users' data";
        }
        else
        {
            if (compareRole == null)
                throw new ArgumentNullException(nameof(compareRole),
                    "compareRole cannot be null when comparison is provided.");

            if (userIdFromClaim.Value != userId && !comparison(role.Value, compareRole.Value))
                return "You do not have permission to access other users' data";
        }

        return string.Empty;
    }
}