using System.Text.Json.Serialization;

namespace back.Models
{
	public class Budget
	{
		public int Id { get; set; }
		public DateTime CreateDate { get; set; } = DateTime.Now;
		public DateTime? UpdatedAt { get; set; }
		public DateTime? EndDate { get; set; } = new DateTime(DateTime.Now.Year + 1, 12, 31);


		public int Amount { get; set; }
		public string Message { get; set; } = String.Empty;

		[JsonIgnore]
		public Game Game { get; set; }
	}
}
