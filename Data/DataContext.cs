using back.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace back.data
{
	public class DataContext : IdentityDbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) {
		}

		public DbSet<Game> Games { get; set; }
		public DbSet<DevPriority> DevPriorities { get; set; }
		public DbSet<DevStatus> DevStatuses { get; set; }
		public DbSet<GameEngine> GameEngines { get; set; }
		public DbSet<News> Newses { get; set; }
		public DbSet<Genre> Genres { get; set; }
		public DbSet<Platform> Platforms { get; set; }
	}
}
