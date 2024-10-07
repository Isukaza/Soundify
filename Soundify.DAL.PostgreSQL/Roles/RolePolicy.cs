namespace Soundify.DAL.PostgreSQL.Roles;

public enum RolePolicy
{
    RequireAnyAdmin,
    RequireAnyAdminOrPublisher,
    RequireContentEditors
}