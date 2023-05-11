using back.Models;
using Microsoft.EntityFrameworkCore;

namespace back.data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions options) : base(options) {
		}

		public DbSet<News> Newses { get; set; }
	}
}
