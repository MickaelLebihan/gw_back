using back.data;
using back.Dto;
using back.Models;
using back.OtherObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace back.Controllers
{

	public class FavoritesDto
	{
		public int GameId { get; set; }
	}
	public class UserController : ControllerBase
	{

		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;

		private readonly DataContext _context;

		public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, DataContext context)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;

			_context = context;
		}

		[HttpGet]
		[Route("user")]
		[Authorize]
		public async Task<IActionResult> GetUserInfo()
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			// Utiliser le UserManager pour récupérer l'utilisateur à partir de l'ID
			User user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
				return NotFound();
			}

			var roles = _userManager.GetRolesAsync(user).Result.ToList();

			var userDto = new UserDto
			{
				Username = user.UserName,
				Email = user.Email,
				Roles = roles
			};

			return Ok(userDto);
		}

		[HttpGet]
		[Route("user/favorites")]
		[Authorize(Roles = "USER")]
		public async Task<IActionResult> GetUserFavorites()
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userId))
			{
				return NotFound("L'identifiant de l'utilisateur est introuvable.");
			}

			var user = await _context.Users
				.Include(u => u.Games)
				.FirstOrDefaultAsync(u => u.Id == userId);

			if (user == null)
			{
				return NotFound("L'utilisateur n'existe pas.");
			}

			if (user.Games.Count == 0)
			{
				return Ok(user.Games);
			}

			var favorites = user.Games.ToList();
			return Ok(favorites);
		}

		[HttpPost]
		[Route("user/favorites/add")]
		//[Authorize]
		public async Task<IActionResult> AddToUserFavorites([FromBody] FavoritesDto favorite)
		{

			//return Ok(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			User user = _context.Users.FirstOrDefault(u => u.Id == userId);


			if (user == null)
			{
				return NotFound();
			}

			Game game = await _context.Games.FindAsync(favorite.GameId);

			if (game != null)
			{
				var title = game.Title;
				user.Games.Add(game);

				_context.SaveChanges();
				return Ok(title + " a été ajouté aux favoris");
			}

			var data = new { favorite.GameId, game };

			return NotFound(data);

		}
		
		[HttpPost]
		[Route("user/favorites/remove")]
		//[Authorize]
		public async Task<IActionResult> RemoveFromUserFavorites([FromBody] FavoritesDto favorite)
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			User user = _context.Users.Include(u => u.Games).FirstOrDefault(u => u.Id == userId);

			if (user == null)
			{
				return NotFound();
			}

			Game game = user.Games.FirstOrDefault(g =>  g.Id == favorite.GameId);

			if (game != null)
			{
				var title = game.Title;
				user.Games.Remove(game);

				_context.SaveChanges();
				return Ok(title + " a été retiré des favoris");
			}

			var data = new{ favorite.GameId, game };

			return NotFound(data);

		}
	}
}
