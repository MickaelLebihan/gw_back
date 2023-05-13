using back.data;
using back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back.Controllers
{
	[ApiController]
	[Route("api/")]
	public class NewsController : ControllerBase
	{
		private readonly DataContext _context;

		public NewsController(DataContext context)
		{
			_context = context;
		}

		[HttpGet]
		[Route("news")]
		public async Task<ActionResult<List<News>>> GetAllNews()
		{
			var newses = await _context.Newses.ToListAsync();
			

			return Ok(newses);
		}
	}
}
