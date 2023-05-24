using System.ComponentModel.DataAnnotations;

namespace back.Dto
{
	public class UpdatePermissionDto
	{
		[Required(ErrorMessage = "Le nom d'utilisateur est requis")]
		public string UserName { get; set; }
	}
}
