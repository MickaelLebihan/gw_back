using back.data;
using back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back.Controllers
{
	[ApiController]
	[Route("/api")]
	public class GameEngineController : ControllerBase
	{
		private readonly DataContext _context;

		public GameEngineController(DataContext context)
		{
			_context = context;
		}

		[HttpGet]
		[Route("gameEngine")]
		public async Task<ActionResult<List<GameEngine>>> GetAllGameEngines()
		{
			var gameEngines = await _context.GameEngines.ToListAsync();


			return Ok(gameEngines);
		}
	}
}
