using LimeTech_Components.Server.Data.Models;
using LimeTech_Components.Server.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LimeTech_Components.Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;

        public AuthController(UserManager<Customer> userManager, SignInManager<Customer> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var existingEmail = await _userManager.FindByEmailAsync(registerDTO.Email);

            if (existingEmail != null) 
            {
                return BadRequest(new { message = "Email is already in use." });
            }


            var existingUsername = await _userManager.FindByNameAsync(registerDTO.Username);

            if (existingUsername != null)
            {
                return BadRequest(new { message = "Username is already taken." });
            }

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



        //POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(loginDTO.Username);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid login attempt!" });
            }


            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);


            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "User";

                string? customerId = null;
                if (user is Customer customer)
                {
                    customerId = customer.PublicID;
                }

                return Ok(new
                {
                    userId = user.Id,
                    customerId,
                    role
                });
            }

            return Unauthorized(new { message = "Invalid login attempt!" });
        }


        // POST: api/auth/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully" });
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




    }
}
