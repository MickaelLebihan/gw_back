using System.ComponentModel.DataAnnotations.Schema;

namespace back.Models
{
	[Table("genres")]
	public class Genre
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
