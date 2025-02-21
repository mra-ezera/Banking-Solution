using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

public static class AuthenticationExtensions
{
    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.MapInboundClaims = true;

                var jwtKey = configuration["Jwt:Key"];
                if (string.IsNullOrEmpty(jwtKey))
                {
                    throw new InvalidOperationException("JWT Key is not configured properly.");
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew = TimeSpan.Zero,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    NameClaimType = "unique_name",
                    RoleClaimType = "role"
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = async context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers["Token-Expired"] = "true";
                        }

                        Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                        await Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        try
                        {
                            var accessToken = context.Request.Headers["Authorization"]
                                .ToString()
                                .Replace("Bearer ", string.Empty);

                            var handler = new JwtSecurityTokenHandler();
                            var token = handler.ReadJwtToken(accessToken);

                            if (token != null)
                            {
                                context.HttpContext.Items["JwtToken"] = token;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error in OnTokenValidated: {ex.Message}");
                        }

                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        if (authHeader != null && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        {
                            var token = authHeader.Substring("Bearer ".Length).Trim();
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        return Task.CompletedTask;
                    }
                };
            });
    }
}
