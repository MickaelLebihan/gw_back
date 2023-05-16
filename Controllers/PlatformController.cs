using back.data;
using back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back.Controllers
{
	[ApiController]
	[Route("/api")]
	public class PlatformController : ControllerBase
	{
		private readonly DataContext _context;

		public PlatformController(DataContext context)
		{
			_context = context;
		}

		[HttpGet]
		[Route("platforms")]
		public async Task<ActionResult<List<Platform>>> GetAllPlatforms()
		{
			var platforms = await _context.Platforms.ToListAsync();


			return Ok(platforms);
		}
	}
}