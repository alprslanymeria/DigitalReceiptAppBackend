using System.Net.Http.Headers;
using System.Net.Http.Json;
using App.Application.Common;
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

    // FIELDS
    private readonly UserApiOptions _options = options.Value;

    public async Task<UserDto?> GetProfileInfos(string userId, string accessToken, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("UserApiClient:GetProfileInfos -> FETCHING USER INFO FOR USER {UserId}", userId);

        try
        {

            // PREPARE REQUEST
            var requestUri = $"{_options.UserInfoEndpoint}";

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // SEND REQUEST
            var response = await httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("UserApiClient:GetProfileInfos -> FAILED TO FETCH USER INFO. STATUS: {StatusCode}", response.StatusCode);
                return null;
            }

            // WRITE RESPONSE TO USER DTO
            var responseWrapper = await response.Content.ReadFromJsonAsync<ServiceResult<UserDto>>(cancellationToken);

            var userDto = responseWrapper?.Data;

            logger.LogInformation("UserApiClient:GetProfileInfos -> SUCCESSFULLY FETCHED USER INFO");

            return userDto;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "UserApiClient:GetProfileInfos -> ERROR FETCHING USER INFO FOR USER {UserId}", userId);
            return null;
        }
    }
}
