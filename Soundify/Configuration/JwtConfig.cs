using System.Text;
using Microsoft.IdentityModel.Tokens;

using Helpers;

namespace Soundify.Configuration;

public static class JwtConfig
{
    private static class Keys
    {
        private const string GroupName = "JWT";
        public const string IssuerKey = GroupName + ":Issuer";
        public const string AudienceKey = GroupName + ":Audience";
        public const string KeyKey = GroupName + ":Key";
    }

    public static class Values
    {
        public static string Issuer { get; private set; }
        public static string Audience { get; private set; }
        public static SymmetricSecurityKey SymmetricSecurityKey { get; private set; }

        public static void Initialize(IConfiguration configuration, bool isDevelopment)
        {
            Issuer = DataHelper.GetRequiredString(configuration[Keys.IssuerKey], Keys.IssuerKey);
            Audience = DataHelper.GetRequiredString(configuration[Keys.AudienceKey], Keys.AudienceKey);
            
            var rawJwtKey = isDevelopment
                ? configuration[Keys.KeyKey]
                : Environment.GetEnvironmentVariable("JWT_KEY");

            var key = DataHelper.GetRequiredString(rawJwtKey, Keys.KeyKey, 32);

            SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}