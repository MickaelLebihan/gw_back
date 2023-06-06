using back.Dto;
using back.Models;
using back.OtherObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace back.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;

		public AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
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
		[Route("seed-users")]
		public async Task<IActionResult> SeedUsers()
		{

			var users = new string[]{ "user", "admin", "cm", "producer" };
			foreach(var userName in users)
			{
				User newUser = new User()
				{
					UserName = "fake" + userName,
					Email = "fake." + userName + "@gmail.com",
					SecurityStamp = Guid.NewGuid().ToString(),
				};
			
				var isExistsUser = await _userManager.FindByNameAsync(newUser.UserName);

				if (isExistsUser != null)
				{
					return BadRequest("L'utilisateur existe déjà");
				}

				var password = newUser + "123";

				await _userManager.CreateAsync(newUser, password);
				var role = StaticUserRoles.USER;

				switch (userName)
				{
					case "user":
						role = StaticUserRoles.USER; break;
					case "admin":
						role = StaticUserRoles.ADMIN; break;
					case "cm":
						role = StaticUserRoles.COMMUNITY_MANAGER; break;
					case "producer":
						role = StaticUserRoles.PRODUCER; break;
					default: return BadRequest("Aucun role ne correspond");
				}

				await _userManager.AddToRoleAsync(newUser, role);
			}
			//User admin = new User()
			//{
			//	UserName = "fakeadmin",
			//	Email = "fake.admin@gmail.com",
			//	SecurityStamp = Guid.NewGuid().ToString(),
			//};


			return Ok("les utilisateurs ont bien été crée");
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

			User newUser = new User()
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
				return Unauthorized("nom de connection érronées");

			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

			if (!isPasswordCorrect)
				return Unauthorized("mdp de connection érronées");

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

		//[HttpGet]
		//[Route("user")]
		//[Authorize]
		//public async Task<IActionResult> GetUserInfo()
		//{
		//	string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

		//	// Utiliser le UserManager pour récupérer l'utilisateur à partir de l'ID
		//	 User user = await _userManager.FindByIdAsync(userId);

		//	if (user == null)
		//	{
		//		return NotFound();
		//	}

		//	var roles = _userManager.GetRolesAsync(user).Result.ToList();

		//	var userDto = new UserDto
		//	{
		//		Username = user.UserName,
		//		Email = user.Email,
		//		Roles = roles
		//	};

		//	return Ok(userDto);
		//}

		private string GenerateNewJsonWebToken(List<Claim> claims)
		{
			var tokenExpiration = _configuration["JWT:Expiration"];

			var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

			var tokenObject = new JwtSecurityToken(
				issuer: _configuration["JWT:ValidIssuer"],
				audience: _configuration["JWT:ValidAudience"],
				expires: DateTime.Now.AddHours(Double.Parse(tokenExpiration)),
				claims: claims,
				signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
				);

			string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

			return token;
		}
	}
}
