using System.Net.Http.Headers;
using System.Net.Http.Json;
using App.Application.Contracts.Infrastructure.ExternalApi;
using App.Domain.Options.ExternalAPI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Integration.ExternalApi;

/// <summary>
/// HTTP CLIENT IMPLEMENTATION FOR EXTERNAL USER API (OAUTH SERVER).
/// </summary>
public class UserApiClient(

    HttpClient httpClient,
    IOptions<UserApiOptions> options,
    ILogger<UserApiClient> logger
    
    ) : IUserApiClient
{
    private readonly UserApiOptions _options = options.Value;

    public async Task<UserInfoResponse?> GetUserInfoAsync(string userId, string accessToken, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("UserApiClient:GetUserInfoAsync -> FETCHING USER INFO FOR USER {UserId}", userId);

        try
        {
            var requestUri = $"{_options.UserInfoEndpoint}/{userId}";

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("UserApiClient:GetUserInfoAsync -> FAILED TO FETCH USER INFO. STATUS: {StatusCode}",
                    response.StatusCode);
                return null;
            }

            var userInfo = await response.Content.ReadFromJsonAsync<UserInfoResponse>(cancellationToken);

            logger.LogInformation("UserApiClient:GetUserInfoAsync -> SUCCESSFULLY FETCHED USER INFO");

            return userInfo;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "UserApiClient:GetUserInfoAsync -> ERROR FETCHING USER INFO FOR USER {UserId}", userId);
            return null;
        }
    }
}
