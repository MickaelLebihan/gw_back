using back.data;
using back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back.Controllers
{
	[ApiController]
	[Route("/api")]
	public class DevPriority : ControllerBase
	{
		private readonly DataContext _context;

		public DevPriority(DataContext context)
		{
			_context = context;
		}

		[HttpGet]
		[Route("devPriority")]
		public async Task<ActionResult<List<Game>>> GetAllDevPriorities()
		{
			var devPriority = await _context.DevPriorities.ToListAsync();


			return Ok(devPriority);
		}
	}
}
