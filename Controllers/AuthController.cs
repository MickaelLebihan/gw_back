using back.Dto;
using back.OtherObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace back.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;

		public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
		}

		// Route pour ajouter des Roles a la bdd
		[HttpPost]
		[Route("seed-roles")]
		public async Task<IActionResult> SeedRoles()
		{
			bool isUserRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);
			bool isAdminRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
			bool isProducerRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.PRODUCER);
			bool isCommunityMangerRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.COMMUNITY_MANAGER);

			if (isAdminRoleExist && isCommunityMangerRoleExist && isProducerRoleExist && isUserRoleExist)
			{
				return Ok("Les Roles de base existe déjà");
			}

			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.PRODUCER));
			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.COMMUNITY_MANAGER));

			return Ok("Ajout des Roles de base effectué");
		}

		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
		{
			var isExistsUser = await _userManager.FindByNameAsync(registerDto.UserName);

			if (isExistsUser != null)
			{
				return BadRequest("L'utilisateur existe déjà");
			}

			IdentityUser newUser = new IdentityUser()
			{
				Email = registerDto.Email,
				UserName = registerDto.UserName,
				SecurityStamp = Guid.NewGuid().ToString(),
			};

			var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

			if (!createUserResult.Succeeded)
			{
				var errorString = "L'utilisateur n'as pas été crée car: ";
				foreach ( var error in createUserResult.Errors)
				{
					errorString += " # " + error.Description;
				}
				return BadRequest(errorString);
			}

			await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);

			return Ok("L'utilisateur a été crée avec succés");
		}

		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			var user = await _userManager.FindByNameAsync(loginDto.UserName);

			if (user is null)
				return Unauthorized("informations de connection érronées");

			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

			if (!isPasswordCorrect)
				return Unauthorized("informations de connection érronées");

			var userRoles = await _userManager.GetRolesAsync(user);

			var authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim("JWTID", Guid.NewGuid().ToString()),
			};

			foreach (var userRole in userRoles)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, userRole));
			}

			var token = GenerateNewJsonWebToken(authClaims);

			return Ok(token);
		}

		private string GenerateNewJsonWebToken(List<Claim> claims)
		{
			var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

			var tokenObject = new JwtSecurityToken(
				issuer: _configuration["JWT:ValidIssuer"],
				audience: _configuration["JWT:ValidAudience"],
				expires: DateTime.Now.AddHours(1),
				claims: claims,
				signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
				);

			string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

			return token;
		}
	}
}
