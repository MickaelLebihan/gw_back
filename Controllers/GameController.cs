using back.data;
using back.Dto;
using back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Slugify;

namespace back.Controllers
{

	[ApiController]
	[Route("/api")]
	public class GameController : ControllerBase
	{
		private readonly DataContext _context;

		public GameController(DataContext context)
		{
			_context = context;
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
		[Route("game/{slug}")]
		//public async Task<ActionResult<Game>> GetsingleGame(int id)
		public async Task<ActionResult<Game>> GetsingleGame(string slug)
		{
			var game = await _context.Games.Include(g => g.Platforms).Include(g => g.GameEngine).Include(g => g.Genres).FirstOrDefaultAsync(g => g.Slug_Title == slug);
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
			SlugHelper slugHelper = new SlugHelper();

			var newGame = new Game {

				Title = game.Title,
				Slug_Title = slugHelper.GenerateSlug(game.Title),
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
