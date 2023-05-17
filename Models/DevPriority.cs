using System.ComponentModel.DataAnnotations.Schema;

namespace back.Models
{
	[Table("devpriorities")]
	public class DevPriority
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
