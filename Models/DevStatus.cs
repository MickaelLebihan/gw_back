using System.ComponentModel.DataAnnotations.Schema;

namespace back.Models
{
	[Table("devstatuses")]
	public class DevStatus
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
