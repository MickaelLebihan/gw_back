using System.ComponentModel.DataAnnotations.Schema;

namespace back.Models
{
	[Table("gameengines")]
	public class GameEngine
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
