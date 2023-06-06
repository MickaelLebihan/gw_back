using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace back.Models
{
	[Table("users")]
	public class User : IdentityUser
	{
		public List<Game>? Games { get; set; } = new List<Game>();
	}
}
