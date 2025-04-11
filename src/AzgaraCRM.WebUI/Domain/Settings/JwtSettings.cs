namespace AzgaraCRM.WebUI.Domain.Settings;

public class JwtSettings
{
    public bool ValidateIssuer { get; set; }

    public string ValidIssuer { get; set; } = default!;

    public bool ValidateAudience { get; set; }

    public string ValidAudience { get; set; } = default!;

    public bool ValidateLifeTime { get; set; }

    public int ExpirationTimeInSeconds { get; set; }

    public bool ValidateIssuerSigningKey { get; set; }

    public string SecretKey { get; set; } = default!;
}