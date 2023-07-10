using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace back.Models
{
	[Table("games")]
	public class Game
	{
		public int Id { get; set; }
		public DateTime CreateDate { get; set; } = DateTime.Now;
		public DateTime? UpdatedAt { get; set; }
		public DateTime? EstimatedReleaseDate { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Slug_Title { get; set; }
		public string Description { get; set; } = string.Empty;
		public List<Platform>? Platforms { get; set; } = new List<Platform>();
		public DevPriority? DevPriority { get; set; }
		public int? UsersScore { get; set; }
		public GameEngine? GameEngine { get; set; }
		public List<Budget>? Budgets { get; set; } = new List<Budget>();
		public DevStatus? DevStatus { get; set; }
		public List<Genre>? Genres { get; set; } = new List<Genre>();
		public int? MinPlayer { get; set; }
		public int? MaxPlayer { get; set; }

		[JsonIgnore]
		public List<User> Users { get; set; }
	}
}
