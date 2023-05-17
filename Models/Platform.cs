using System.ComponentModel.DataAnnotations.Schema;

namespace back.Models
{
	[Table("platforms")]
	public class Platform
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
