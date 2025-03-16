﻿using LimeTech_Components.Server.Data.Models;
using LimeTech_Components.Server.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LimeTech_Components.Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly IConfiguration _configuration;
        private static Dictionary<string, string> _refreshTokens = new();

        public AuthController(UserManager<Customer> userManager, SignInManager<Customer> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var existingEmail = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (existingEmail != null) return BadRequest(new { message = "Email is already in use." });

            var existingUsername = await _userManager.FindByNameAsync(registerDTO.Username);
            if (existingUsername != null) return BadRequest(new { message = "Username is already taken." });


            var user = new Customer
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                FullName = registerDTO.FullName
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                return Ok(new { message = "Registration successful!" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }



        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(loginDTO.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                return Unauthorized(new { message = "Invalid login attempt!" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";

            string? customerId = null;
            if (user is Customer customer)
            {
                customerId = customer.CustomerId;
            }

            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();
            _refreshTokens[user.Id] = refreshToken;

            return Ok(new
            {
                userId = user.Id,
                customerId,
                role,
                accessToken,
                refreshToken
            });
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenDTO tokenDTO)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDTO.AccessToken);
            if (principal == null) return Unauthorized(new { message = "Invalid token." });

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized(new { message = "Invalid refresh token." });

            // Fetch refresh token from database
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.RefreshToken != tokenDTO.RefreshToken)
            {
                return Unauthorized(new { message = "Invalid refresh token." });
            }

            // Check expiration
            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized(new { message = "Refresh token expired." });
            }

            // Generate new tokens
            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            // Save new refresh token in database
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Example expiry
            await _userManager.UpdateAsync(user);

            return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
        }


        // POST: api/auth/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null) _refreshTokens.Remove(userId);

            return Ok(new { message = "Logged out successfully" });
        }

        //// POST: api/auth/logout
        //[HttpPost("logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();
        //    return Ok(new { message = "Logged out successfully" });
        //}


        private string GenerateJwtToken(Customer user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }
                return principal;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var existingEmail = await _userManager.FindByEmailAsync(email);
            if (existingEmail != null)
            {
                return BadRequest(new { message = "Email is already in use" });
            }
            return Ok();
        }



        [HttpGet("check-username")]
        public async Task<IActionResult> CheckUsername(string username)
        {
            var existingUsername = await _userManager.FindByNameAsync(username);
            if (existingUsername != null)
            {
                return BadRequest(new { message = "Username is already taken" });
            }
            return Ok();
        }


        // GET: api/auth/validate-session
        [HttpGet("validate-session")]
        [Authorize]
        public IActionResult ValidateSession()
        {
            var user = HttpContext.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                return Ok(true);
            }
            return Unauthorized();
        }



    }
}
