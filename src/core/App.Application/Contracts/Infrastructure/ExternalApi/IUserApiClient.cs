namespace App.Application.Contracts.Infrastructure.ExternalApi;

/// <summary>
/// RESPONSE MODEL FOR USER INFO FROM EXTERNAL API.
/// </summary>
public record UserDto(

    string Id,
    string? UserName,
    string Email,
    string? ImageUrl);

/// <summary>
/// INTERFACE FOR EXTERNAL USER API CLIENT (OAUTH SERVER).
/// </summary>
public interface IUserApiClient
{
    /// <summary>
    /// RETRIEVES USER INFORMATION FROM THE EXTERNAL OAUTH SERVER.
    /// </summary>
    Task<UserDto?> GetProfileInfos(string userId, string accessToken, CancellationToken cancellationToken = default);
}
