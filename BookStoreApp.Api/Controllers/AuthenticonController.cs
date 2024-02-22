using AutoMapper;
using BookStoreApp.Api.Data;
using BookStoreApp.Api.Models.User;
using BookStoreApp.Api.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.Api.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	[AllowAnonymous] // not sure if need this yet ?
	public class AuthenticonController : ControllerBase
	{
		// ILogger<AuthenticonController>: This part declares the type of the logger field.
		// It's a generic interface called ILogger, but it's constrained to the specific type AuthenticonController.
		// This means the logger can only hold an object that implements the ILogger interface -
		// and logs messages related to the AuthenticonController class.

		private readonly ILogger<AuthenticonController> logger;
		private readonly IMapper mapper;
		private readonly UserManager<ApiUser> userManager;
		private readonly IConfiguration configuration;

		public AuthenticonController(ILogger<AuthenticonController> logger, IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
			this.logger = logger;
			this.mapper = mapper;
			this.userManager = userManager;
			this.configuration = configuration;
		}

		// Action represents different behaviors at endpoints

		// Use post to when the api accepts data from a form the user filled in
		// Create a new user
		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register(CreateUserDto createUserDto)
		{
			logger.LogInformation($"Registration Attempt for {createUserDto.Email}");
			try
			{
				var user = mapper.Map<ApiUser>(createUserDto);
				user.UserName = createUserDto.Email;
				var result = await userManager.CreateAsync(user, createUserDto.Password);

				if (result.Succeeded == false)
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(error.Code, error.Description);
					}
					return BadRequest(ModelState);
				}
				
				await userManager.AddToRoleAsync(user, "User");
				return Accepted();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Something Went Wtrong in the {nameof(Register)}");
				return Problem($"Something Went Wtrong in the {nameof(Register)}", statusCode: 500);
			}							
		}

		[HttpPost] 
		[Route("login")]
		public async Task<ActionResult<AuthenticationRepsonse>> Login(LoginUserDto loginUserDto)
		{
			logger.LogInformation($"Login Attempt for {loginUserDto.Email}");
			try
			{
				var user = await userManager.FindByEmailAsync(loginUserDto.Email);
				var passwordValid = await userManager.CheckPasswordAsync(user, loginUserDto.Password);

				if (user == null || passwordValid == false)
				{
					return Unauthorized(loginUserDto); // 401
				}

				string tokenString = await GenerateToken(user);

				var response = new AuthenticationRepsonse
				{
					Email = loginUserDto.Email,
					Token = tokenString,
					UserId = user.Id
				};

				return response;
				//return Accepted(response); // can be just repsonse video	32 20 minutes			
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Something Went Wtrong in the {nameof(Register)}");
				return Problem($"Something Went Wtrong in the {nameof(Register)}", statusCode: 500);
			}
		}

		private async Task<string> GenerateToken(ApiUser user)
		{		
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var roles = await userManager.GetRolesAsync(user);
			var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList(); // q represents each string in the list

			var userClaims = await userManager.GetClaimsAsync(user);
			
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(CustomClaimTypes.UserId, user.Id), // from static class
			}
			.Union(userClaims)
			.Union(roleClaims);

			var token = new JwtSecurityToken(
				issuer: configuration["JwtSettings:Issuer"],
				audience: configuration["JwtSettings:Audience"], 
				claims: claims,
				expires: DateTime.UtcNow.AddHours(Convert.ToInt32(configuration["JwtSettings:Duration"])),
				signingCredentials: credentials
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
