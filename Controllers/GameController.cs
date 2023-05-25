using back.data;
using back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

			return Ok(games);
		}

		[HttpGet]
		[Route("game/{id}")]
		public async Task<ActionResult<Game>> GetsingleGame(int id)
		{
			var game = await _context.Games.Include(g => g.Platforms).Include(g => g.GameEngine).Include(g => g.Genres).FirstOrDefaultAsync(g => g.Id == id);
	    return Ok(game);
		}
    
    
		[HttpPost]
		[Route("game/add")]
		public async Task<ActionResult<List<Game>>>AddGame(Game game)
		{
			//var game = new Game();
			await _context.Games.AddAsync(game);
			_context.SaveChanges();


			return Ok(game);
		}
	}
}
