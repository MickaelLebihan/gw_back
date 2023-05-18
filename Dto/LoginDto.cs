using System.ComponentModel.DataAnnotations;

namespace back.Dto
{
	public class LoginDto
	{
		[Required(ErrorMessage = "Le nom d'utilisateur est requis")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Le mot de passe est requis")]
		public string Password { get; set; }

	}
}
