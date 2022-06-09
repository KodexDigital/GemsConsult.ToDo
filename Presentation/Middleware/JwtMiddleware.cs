using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Todo.Core.Settings;
using Todo.Services.Declarations;

namespace Presentation.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings jwtSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<JwtSettings> jwtSettings)
        {
            _next = next;
            this.jwtSettings = jwtSettings.Value;
        }

        public async Task Invoke(HttpContext context, IApplicationUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AttachUserToContext(context, userService, token);

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, IApplicationUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtSettings.AuthKey);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "userId").Value;

                // attach user to context on successful jwt validation
                context.Items["User"] = userService.SingleUserDetailAsync(userId);
            }
            catch { }
        }
    }
}
