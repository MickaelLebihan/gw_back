namespace back.Dto
{
	public class GameDto
	{
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int? DevPriority { get; set; }
		public int? UsersScore { get; set; }
		public int? GameEngine { get; set; }
		public List<int>? Platforms { get; set; } = new List<int>();
		public List<int>? Genres { get; set; } = new List<int>();
		public int? Budget { get; set; }
		public int? DevStatus { get; set; }
		public int? MinPlayer { get; set; }
		public int? MaxPlayer { get; set; }

		// Si on souhaite ajouter de nouvelle Entité aux plateformes, moteurs ou genres
		public Boolean NewEngine { get; set; } = false;
		public string? NewEngineName { get; set; } = string.Empty;
		public Boolean NewPlatform { get; set; } = false;
		public string? NewPlatformName { get; set; } = string.Empty;
		public Boolean NewGenre { get; set; } = false;
		public string? NewGenreName { get; set; } = string.Empty;

		public GameDto()
		{
			if (GameEngine == null)
				GameEngine = null;
			if (Platforms == null)
				Platforms = null;
			if (Genres == null)
				Genres = null;
			if (NewEngine == null)
				NewEngineName = null;
			if (NewPlatform == null)
				NewPlatformName = null;
			if (NewGenre == null)
				NewGenreName = null;
		}

	}
}
