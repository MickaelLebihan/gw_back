using back.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Query.Internal;

namespace back.data
{
	public class DataContext : IdentityDbContext<User>
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) {
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<IdentityUserRole<string>>()
				.ToTable("userroles");

			modelBuilder.Entity<IdentityRole>(entity =>
			{
				entity.ToTable("roles");
			});

			modelBuilder.Entity<User>(entity =>
			{
				entity.ToTable("users");
			});

			modelBuilder.Entity<User>()
				.HasMany(u => u.Games)
				.WithMany(g => g.Users)
				.UsingEntity("userfavorites");

			modelBuilder.Entity<Game>()
				.HasMany(g => g.Platforms)
				.WithMany(p => p.Games)
				.UsingEntity("gameplatform");

			modelBuilder.Entity<Game>()
				.HasMany(g => g.Genres)
				.WithMany(p => p.Games)
				.UsingEntity("gamegenre");

			//modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
			//modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();
			//modelBuilder.Entity< IdentityUserToken< string>>().HasNoKey();

			modelBuilder.Entity<IdentityUserLogin<string>>()
			.HasKey(l => new { l.LoginProvider, l.ProviderKey });

			modelBuilder.Entity<IdentityUserRole<string>>()
				.HasKey(r => new { r.UserId, r.RoleId });
			
			modelBuilder.Entity<IdentityUserToken<string>>()
				.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
		}

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	base.OnConfiguring(optionsBuilder);

		//	IConfigurationRoot configuration = new ConfigurationBuilder()
		//		.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
		//		.AddJsonFile("appsettings.json")
		//		.Build();

		//	string connectionString = configuration.GetConnectionString("DefaultConnection");
		//	var serverVersion = ServerVersion.AutoDetect(connectionString);



			//optionsBuilder.UseMySql(serverVersion.ToString(), connectionString, mySqlOptions =>
			//{
			//	mySqlOptions.UseLowerCaseTableNameConvention();
			//})
			//.ReplaceService<IMethodCallTranslatorProvider, MySqlMethodCallTranslatorProvider>();
		//}

		public DbSet<Game> Games { get; set; }
		public DbSet<DevPriority> DevPriorities { get; set; }
		public DbSet<DevStatus> DevStatuses { get; set; }
		public DbSet<GameEngine> GameEngines { get; set; }
		public DbSet<News> Newses { get; set; }
		public DbSet<Genre> Genres { get; set; }
		public DbSet<Platform> Platforms { get; set; }
	}
}
