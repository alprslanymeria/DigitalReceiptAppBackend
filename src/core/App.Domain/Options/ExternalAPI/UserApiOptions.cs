namespace App.Domain.Options.ExternalAPI;

/// <summary>
/// CONFIGURATION OPTIONS FOR EXTERNAL USER API (OAUTH SERVER).
/// </summary>
public class UserApiOptions
{
    public const string Key = "UserApi";

    /// <summary>
    /// BASE URL OF THE OAUTH/USER API SERVER.
    /// </summary>
    public string BaseUrl { get; set; } = null!;

    /// <summary>
    /// ENDPOINT PATH FOR GETTING USER INFO (E.G., "/api/users/{userId}").
    /// </summary>
    public string UserInfoEndpoint { get; set; } = "/api/users";

    /// <summary>
    /// TIMEOUT IN SECONDS FOR API CALLS.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
}
