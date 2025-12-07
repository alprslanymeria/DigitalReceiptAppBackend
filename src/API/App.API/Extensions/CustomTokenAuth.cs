using App.Domain.Options;
using App.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace App.API.Extensions;

public static class CustomTokenAuth
{
    public static IServiceCollection AddCustomTokenAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
        {
            var tokenOptions = configuration.GetSection(TokenOption.Key).Get<TokenOption>();

            opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidIssuer = tokenOptions!.Issuer,
                ValidAudience = tokenOptions.Audience[0],
                IssuerSigningKey = SignService.GetSymetricKey(tokenOptions.SecurityKey),

                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
}