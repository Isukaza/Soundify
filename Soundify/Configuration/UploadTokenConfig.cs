using System.Text;
using Microsoft.IdentityModel.Tokens;
using Helpers;

namespace Soundify.Configuration;

public static class UploadTokenConfig
{
    private static class Keys
    {
        private const string GroupName = "UploadToken";
        public const string AesKeyKey = GroupName + ":AesKey";
        public const string ExpiresKey = GroupName + ":Expires";
    }

    public static class Values
    {
        public static string AesKey { get; private set; }
        public static SigningCredentials JwtKey { get; private set; }
        public static TimeSpan Expires { get; private set; }

        public static void Initialize(IConfiguration configuration, bool isDevelopment)
        {
            var rawAesKey = isDevelopment
                ? configuration[Keys.AesKeyKey]
                : Environment.GetEnvironmentVariable("UPLOAD_TOKEN_AES_KEY");

            AesKey = DataHelper.GetRequiredString(rawAesKey, Keys.AesKeyKey, 32);

            JwtKey = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AesKey)),
                SecurityAlgorithms.HmacSha256
            );

            Expires = DataHelper.GetValidatedTimeSpan(configuration[Keys.ExpiresKey], Keys.ExpiresKey, 1, 5);

            var tokenLifetimeStr = configuration[Keys.ExpiresKey];
            if (!int.TryParse(tokenLifetimeStr, out var tokenLifetimeMinutes) || tokenLifetimeMinutes <= 0)
                throw new InvalidOperationException($"Invalid or missing '{Keys.ExpiresKey}' configuration value.");
        }
    }
}