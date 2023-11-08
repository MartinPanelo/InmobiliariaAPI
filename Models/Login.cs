using System.ComponentModel.DataAnnotations;


namespace API.Models
{
	public class LoginView
	{
		[Required(ErrorMessage = "El Usuario es obligatorio."),DataType(DataType.EmailAddress)]
		public string Usuario { get; set; }
		[Required(ErrorMessage = "La contraseña es obligatoria."),DataType(DataType.Password)]
		public string Clave { get; set; }
	}
}
