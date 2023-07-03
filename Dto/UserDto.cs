namespace back.Dto
{
	public class UserDto
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public List<String> Roles { get; set; }
		public List<int> FavoriteGames { get; set; }
	}
}
