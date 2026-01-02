namespace App.Application.Contracts.Infrastructure.ExternalApi;

/// <summary>
/// RESPONSE MODEL FOR USER INFO FROM EXTERNAL API.
/// </summary>
public sealed record UserInfoResponse
{
    /// <summary>
    /// THE USER'S NATIVE LANGUAGE ID.
    /// </summary>
    public int NativeLanguageId { get; init; }
}

/// <summary>
/// INTERFACE FOR EXTERNAL USER API CLIENT (OAUTH SERVER).
/// </summary>
public interface IUserApiClient
{
    /// <summary>
    /// RETRIEVES USER INFORMATION FROM THE EXTERNAL OAUTH SERVER.
    /// </summary>
    /// <param name="userId">THE USER ID TO LOOK UP.</param>
    /// <param name="accessToken">THE ACCESS TOKEN FOR AUTHORIZATION.</param>
    /// <param name="cancellationToken">CANCELLATION TOKEN.</param>
    /// <returns>USER INFORMATION INCLUDING NATIVE LANGUAGE ID.</returns>
    Task<UserInfoResponse?> GetUserInfoAsync(string userId, string accessToken, CancellationToken cancellationToken = default);
}
