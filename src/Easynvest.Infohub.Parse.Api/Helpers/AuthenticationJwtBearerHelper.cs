using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Easynvest.Infohub.Parse.Api.Helpers
{
    public static class AuthenticationJwtBearerHelper
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwtBearerOptions =>
            {
                string issuer = config["Authentication:Issuer"];
                string audience = config["Authentication:Audience"];

                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Base64Encode()),
                };
            });

            return services;
        }

        public static byte[] Base64Encode()
        {
            var plainText = "aQkoPric9NK7eNLX0gI1eu+aZ+V2FyJlks7jfZOsECCBFg3zqboTO2pNKGklQ9YjIqiwhRGTelsCqCV9oguTHfxPx2mqo14bP8ZofDbmRCo7WqFWSUXcBo3tgn96mwXjGFQszJQvLGCcMEjEWuAM8cs4C5oZZD0euUwNo2AW+t5wCQyfWOXpdlnsUzfXzzTzG3IJgvy2GztuFs9lrzUzNAeo5Z0tAODu4NG43PSLGt7tzLa4MSpI4ueoSEdNjYOaDIjMCi9vsXkBt6XQUidC4un3/xmLYBVL8jT+T0Hxb3yOCKcVDLX4HJB9gqQ9SjNPFWmhx+VuAnNYkjN5xRV5nA==";
            return WebEncoders.Base64UrlDecode(plainText);
        }
    }
}
