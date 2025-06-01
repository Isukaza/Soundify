namespace Helpers;

public static class DataHelper
{
    private const int MaxLength = 255;

    /// <summary>
    /// Validates and retrieves a required string setting, with optional length validation.
    /// </summary>
    /// <param name="valueFromConfiguration">The configuration value to validate.</param>
    /// <param name="settingName">A friendly name for the setting (used in exception messages).</param>
    /// <param name="minLength">Optional minimum length for the string.</param>
    /// <returns>The validated configuration value.</returns>
    /// <exception cref="ArgumentException">Thrown if the value is missing, invalid, or violates length constraints.</exception>
    public static string GetRequiredString(string? valueFromConfiguration, string settingName, int? minLength = null)
    {
        if (string.IsNullOrWhiteSpace(valueFromConfiguration))
            throw new ArgumentException($"{settingName} is missing");

        if (minLength.HasValue && valueFromConfiguration.Length < minLength.Value)
            throw new ArgumentException($"{settingName} is too short. Minimum length is {minLength.Value} characters.");

        if (valueFromConfiguration.Length > MaxLength)
            throw new ArgumentException($"{settingName} is too long. Maximum length is {MaxLength} characters.");

        return valueFromConfiguration;
    }

    /// <summary>
    /// Validates and retrieves a required integer setting within a specified range.
    /// </summary>
    /// <param name="valueFromConfiguration">The string value to convert to an integer.</param>
    /// <param name="settingName">A friendly name for the setting (used in exception messages).</param>
    /// <param name="min">The minimum acceptable value for the setting.</param>
    /// <param name="max">The maximum acceptable value for the setting.</param>
    /// <returns>The validated integer value.</returns>
    /// <exception cref="ArgumentException">Thrown if the value is missing, invalid, or out of range.</exception>
    public static int GetRequiredInt(string? valueFromConfiguration, string settingName, int min, int max)
    {
        if (string.IsNullOrWhiteSpace(valueFromConfiguration))
            throw new ArgumentException($"{settingName} is missing");

        if (!int.TryParse(valueFromConfiguration, out var result))
            throw new ArgumentException($"{settingName} is invalid. It must be a valid integer.");

        if (result < min)
            throw new ArgumentException($"{settingName} is too low. The minimum value is {min}.");

        if (result > max)
            throw new ArgumentException($"{settingName} is too high. The maximum value is {max}.");

        return result;
    }

    /// <summary>
    /// Validates and converts a string configuration value to a TimeSpan, ensuring the value is within the specified bounds (in minutes).
    /// The configuration value can be either a plain integer representing minutes or a valid TimeSpan string.
    /// </summary>
    /// <param name="valueFromConfiguration">The configuration value to validate, which can either be a string representing minutes or a TimeSpan string.</param>
    /// <param name="settingName">A friendly name for the setting (used in exception messages).</param>
    /// <param name="min">The minimum allowed value in minutes. The input value will be validated against this minimum.</param>
    /// <param name="max">The maximum allowed value in minutes. The input value will be validated against this maximum.</param>
    /// <returns>A TimeSpan representing the validated value, which is within the specified range in minutes.</returns>
    /// <exception cref="ArgumentException">Thrown if the value is missing, invalid, or out of bounds (not within the specified range in minutes).</exception>
    public static TimeSpan GetValidatedTimeSpan(string? valueFromConfiguration, string settingName, int min, int max)
    {
        if (string.IsNullOrWhiteSpace(valueFromConfiguration))
            throw new ArgumentException($"{settingName} is missing");

        if (int.TryParse(valueFromConfiguration, out var value))
        {
            if (value < min || value > max)
                throw new ArgumentException($"{settingName} should be between {min} and {max} minutes");

            return TimeSpan.FromMinutes(value);
        }

        if (TimeSpan.TryParse(valueFromConfiguration, out var timeSpan))
        {
            var minutes = (int)Math.Ceiling(timeSpan.TotalMinutes);
            if (minutes < min || minutes > max)
                throw new ArgumentException($"{settingName} should be between {min} and {max} minutes");

            return timeSpan;
        }

        throw new ArgumentException($"{settingName} is invalid or missing");
    }
}