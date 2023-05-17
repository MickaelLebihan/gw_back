using back.data;
using back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back.Controllers
{
	[ApiController]
	[Route("/api")]
	public class DevStatus : ControllerBase
	{
		private readonly DataContext _context;

		public DevStatus(DataContext context)
		{
			_context = context;
		}

		[HttpGet]
		[Route("devStatus")]
		public async Task<ActionResult<List<Game>>> GetAllDevStatuses()
		{
			var devStatus = await _context.DevStatuses.ToListAsync();


			return Ok(devStatus);
		}
	}
}
