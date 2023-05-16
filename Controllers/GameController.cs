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
			var games = await _context.Games.ToListAsync();


			return Ok(games);
		}
	}
}
