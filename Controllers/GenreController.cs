using back.data;
using back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back.Controllers
{
	[ApiController]
	[Route("/api")]
	public class GenreController : ControllerBase
	{
		private readonly DataContext _context;

		public GenreController(DataContext context)
		{
			_context = context;
		}

		[HttpGet]
		[Route("genres")]
		public async Task<ActionResult<List<Genre>>> GetAllGenres()
		{
			var genres = await _context.Genres.ToListAsync();


			return Ok(genres);
		}
	}
}
