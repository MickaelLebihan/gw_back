using System.ComponentModel.DataAnnotations;

namespace back.Dto
{
	public class RegisterDto
	{
		[Required(ErrorMessage = "Le nom d'utilisateur est requis")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "L'email est requis")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Le mot de passe est requis")]
		public string Password { get; set; }
	}
}
