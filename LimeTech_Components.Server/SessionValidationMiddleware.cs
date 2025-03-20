namespace LimeTech_Components.Server
{
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    public class SessionValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly SymmetricSecurityKey _signingKey;

        public SessionValidationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized: No token provided.");
                return;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = _signingKey,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                await _next(context); // Token is valid, proceed to the next middleware
            }
            catch (Exception)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized: Invalid or expired token.");
            }
        }
    }


}
