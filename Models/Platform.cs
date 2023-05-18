using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace back.Models
{
	[Table("platforms")]
	public class Platform
	{
		public int Id { get; set; }
		public string Name { get; set; }

		[JsonIgnore]
		public List<Game> Games { get; set; }
	}
}
