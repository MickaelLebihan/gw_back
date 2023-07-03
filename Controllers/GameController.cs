using back.data;
using back.Dto;
using back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Slugify;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;

namespace back.Controllers
{

	public class GameFilter
	{
		public string Title { get; set; } = string.Empty;
		public List<int> Genres { get; set; } = new List<int>();
		public List<int> Platforms { get; set; } = new List<int>();
		public int GameEngine { get; set; }
		public int DevStatus { get; set; } = 0;
	}

	public class GamesListDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Slug_Title { get; set; }
		public List<int> Genres { get; set; } = new List<int>();
		public List<int> Platforms { get; set; } = new List<int>();
		public int GameEngine { get; set; }
		public string Description { get; set; }
		public int Min_player { get; set; }
		public int Max_player { get; set; }

	}

	[ApiController]
	[Route("/api")]
	public class GameController : ControllerBase
	{
		private readonly DataContext _context;

		public GameController(DataContext context)
		{
			_context = context;
		}

		//[HttpGet]
		//[Route("games")]
		//public async Task<ActionResult<List<Game>>> GetGames()
		//{

		//	var games = await _context.Games.Include(g => g.Platforms).Include(g => g.GameEngine).Include(g => g.Genres).ToListAsync();
		//	//var games = await _context.Games.ToListAsync().FindAsync();

		//	return Ok(games);
		//}

		[HttpGet]
		[Route("games")]
		public async Task<ActionResult<List<Game>>> FilterGames([FromQuery] GameFilter gamefilter)
		{

			IQueryable<Game> gamesQuery = _context.Games.Include(g => g.Platforms).Include(g => g.GameEngine);

			if (!string.IsNullOrEmpty(gamefilter.Title))
			{
				gamesQuery = gamesQuery.Where(g => g.Title.Contains(gamefilter.Title));
			}

			if (gamefilter.Genres != null && gamefilter.Genres.Count > 0)
			{
				gamesQuery = gamesQuery.Where(g => g.Genres.Any(genre => gamefilter.Genres.Contains(genre.Id)));
			}

			if (gamefilter.Platforms != null && gamefilter.Platforms.Count > 0)
			{
				gamesQuery = gamesQuery.Where(g => g.Platforms.Any(platform => gamefilter.Platforms.Contains(platform.Id)));
			}

			if (gamefilter.GameEngine != 0)
			{
				gamesQuery = gamesQuery.Where(g => g.GameEngine.Id == gamefilter.GameEngine);
			}

			var games = await gamesQuery.ToListAsync();

		//	var gamesList = games.Select(entity => new GamesListDto
		//	{
		//		  Id = entity.Id,
		//		  Title = entity.Title,
		//		  Slug_Title = entity.Slug_Title,
		//		  Genres = entity.Genres,
		//		  Platforms = entity.Platforms,
		//		  GameEngine = (int)entity.GameEngine,
		//		  Description = entity.Description,
		//		  Min_player = (int)entity.MinPlayer,
		//		  Max_player = (int)entity.MaxPlayer
		//}).ToList();

		//	return Ok(gamesList);
			return Ok(games);
		}

		[HttpGet]
		[Route("game_aux_data")]
		public async Task<ActionResult<Dictionary<string, object>>> GetAuxData()
		{
			Dictionary<string, object> auxData = new();

			var platforms = await _context.Platforms.ToListAsync();
			var genres = await _context.Genres.ToListAsync();
			var devStatuses = await _context.DevStatuses.ToListAsync();
			var gameEngines = await _context.GameEngines.ToListAsync();

			auxData.Add("platforms", platforms);
			auxData.Add("genres", genres);
			auxData.Add("devstatuses", devStatuses);
			auxData.Add("gameengines", gameEngines);

			return Ok(auxData);
		}



		[HttpGet]
		[Route("game/{slug}")]
		//public async Task<ActionResult<Game>> GetsingleGame( id)
		public async Task<ActionResult<Game>> GetsingleGame(string slug)
		{
			

			var game = await _context.Games
				.Include(g => g.Platforms)
				.Include(g => g.GameEngine)
				.Include(g => g.Genres)
				.Include(g => g.Users)
				.FirstOrDefaultAsync(g => g.Slug_Title == slug);

			if (game == null)
			{
				return NotFound();
			}

			var options = new JsonSerializerOptions
			{
				ReferenceHandler = ReferenceHandler.Preserve
			};

			return Content(JsonSerializer.Serialize(game, options), "application/json");
		}

		[HttpGet]
		[Route("game/{slug}/countFavorites")]
		public async Task<ActionResult<List<Game>>> CountGamesFavorites(string slug)
		{
			var game = await _context.Games
				.Include(g => g.Users)
				.FirstOrDefaultAsync(g => g.Slug_Title == slug);

			var count = game.Users.Count();
			return Ok(count);
		}


		[HttpPost]
		[Route("game/add")]
		//[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<Game>>AddGame([FromBody] GameDto game)
		{
			SlugHelper slugger = new SlugHelper();

			var newGame = new Game {

				Title = game.Title,
				Slug_Title = slugger.GenerateSlug(game.Title),
				Description = game.Description,

				MinPlayer = game.MinPlayer,
				MaxPlayer = game.MaxPlayer,
			};

			if(game.Platforms != null)
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
	}
}
