namespace back.Models
{
	public class News
	{
		public int Id { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
		public DateTime? PublishedDate { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
	}
}
