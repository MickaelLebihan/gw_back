using back.data;
using back.Dto;
using back.Migrations;
using back.Models;
using back.OtherObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Slugify;
using System.Linq;
using System.Security.Claims;

namespace back.Controllers
{

	[ApiController]
	[Route("/api")]
	public class GameController : ControllerBase
	{
		private readonly DataContext _context;

		private readonly IHttpContextAccessor _httpContextAccessor;


		public GameController(IHttpContextAccessor httpContextAccessor, DataContext context)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
		}

		[HttpGet]
		[Route("games")]
		public async Task<ActionResult<List<Game>>> GetAllGames()
		{
			var games = await _context.Games.Include(g => g.Platforms).Include(g => g.GameEngine).Include(g => g.Genres).ToListAsync();

			SlugHelper slugger = new SlugHelper();

			foreach (Game game in games)
			{
				if (game.Slug_Title == null)
				{
					game.Slug_Title = slugger.GenerateSlug(game.Title);
					_context.SaveChanges();
				}
			}

			return Ok(games);
		}

        [HttpGet]
        [Route("games")]
        public async Task<ActionResult<List<Game>>> Test()
        {

            return Ok("ok !");
        }

        [HttpGet]
		[Route("game/{slug}")]
		public async Task<ActionResult<Game>> GetsingleGame(string slug)
		{
			var user = _httpContextAccessor.HttpContext.User;

			var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

			if(roles.Contains("ADMIN") || roles.Contains("PRODUCER")) {
			
				var gameWithBudget = await _context.Games
					.Include(g => g.Platforms)
					.Include(g => g.GameEngine)
					.Include(g => g.Budgets)
					.Include(g => g.Genres)
					.FirstOrDefaultAsync(g => g.Slug_Title == slug);

				return Ok(gameWithBudget);
			}

			var game = await _context.Games
				.Include(g => g.Platforms)
				.Include(g => g.GameEngine)
				.Include(g => g.Genres)
				.FirstOrDefaultAsync(g => g.Slug_Title == slug);

			return Ok(game);
		}

		[HttpGet]
		[Route("game/{slug}/countFavorites")]
		public async Task<ActionResult<List<Game>>> CountGamesFavorites(string slug)
		{
			var game = await _context.Games.Include(g => g.Users).FirstOrDefaultAsync(g => g.Slug_Title == slug);
			var count = game.Users.Count();
			return Ok(count);
		}


		[HttpPost]
		[Route("game/add")]
		//[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<Game>>AddGame([FromBody] GameDto game)
		{
			SlugHelper slugger = new SlugHelper();

			var slug_title = slugger.GenerateSlug(game.Title);
			var gameExist = _context.Games.Find(slug_title);


			if (gameExist != null)
			{
				return Ok("Ce jeu existe déjà dans la base de donneés !");
			}

			var newGame = new Game {

				Title = game.Title,
				Slug_Title = slug_title,
				Description = game.Description,

				MinPlayer = game.MinPlayer,
				MaxPlayer = game.MaxPlayer,
			};

			if(game.Budget != null)
			{
				var budget = new Budget
				{
					Amount = (int)game.Budget,
					Message = "initial budget"
				};
				_context.Budgets.Add(budget);
				newGame.Budgets.Add(budget);
			}

			if (game.Platforms != null)
			{
				foreach(int platformId in game.Platforms)
					{
						var platform = await _context.Platforms.FirstOrDefaultAsync(g => g.Id == platformId);
						newGame.Platforms.Add(platform);
					}
			} else {
				var newPlatform = new Platform
				{
					Name = game.NewPlatformName
				};
				_context.Platforms.Add(newPlatform);
				newGame.Platforms.Add(newPlatform);
			}
			
			
			if(game.Genres != null)
			{
				foreach(int genreId in game.Genres)
				{
					var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == genreId);
					newGame.Genres.Add(genre);
				}
			} else
			{
				var newGenre = new Genre
				{
					Name = game.NewGenreName
				};
				_context.Genres.Add(newGenre);
				newGame.Genres.Add(newGenre);
			}

			if (game.GameEngine != null)
			{
				var engine = await _context.GameEngines.FirstOrDefaultAsync(g => g.Id == game.GameEngine);
				newGame.GameEngine = engine;
			}
			else
			{
				var newEngine = new GameEngine
				{
					Name = game.NewEngineName
				};
				_context.GameEngines.Add(newEngine);
				newGame.GameEngine = newEngine;
			}


			await _context.Games.AddAsync(newGame);
			_context.SaveChanges();


			return Ok(newGame);
		}

		[HttpPost]
		[Route("game/{slug}/addBudget")]
		[Authorize(Roles = StaticUserRoles.PRODUCER)]
		public async Task<ActionResult<Game>> EditBudget(string slug, BudgetDto budgetDto)
		{

			var game = await _context.Games.Include(g => g.Budgets).FirstOrDefaultAsync(g => g.Slug_Title == slug);

			if (game?.Budgets == null)
			{
				DateTime newEndDate;

				if (budgetDto.EndDate == null)
				{
					newEndDate = new DateTime(DateTime.Now.Year + 1, 12, 31);
				} else
				{
					newEndDate = budgetDto.EndDate;
				}

				var budget = new Budget
				{
					Amount = budgetDto.Amount,
					Message = budgetDto.Message,
					EndDate = newEndDate
				};

				_context.Budgets.Add(budget);
				game.Budgets.Add(budget);
			}


			_context.SaveChanges();


			return Ok(true);
		}
	}
}
