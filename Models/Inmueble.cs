using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models{

	[Table("Inmueble")]
    public class Inmueble{
        [Key]
		[Display(Name = "Identificador")]
        public int IdInmueble {get; set;}
		[Display(Name = "Cantidad de ambientes")]
		[RegularExpression(@"^\d+$", ErrorMessage = "Debe ser un número entero.")]
		[Required(ErrorMessage = "Este campo es obligatorio.")]
		public int Ambientes { get; set; }
		[Required(ErrorMessage = "Este campo es obligatorio.")]
		public string? Uso { get; set; }//tabla o enum
		[Required(ErrorMessage = "Este campo es obligatorio.")]
		public string? Tipo { get; set; }//tabla o enum
		[Required(ErrorMessage = "Este campo es obligatorio.")]
		[Display(Name = "Dirección")]
		public string? Direccion { get; set; }	
		 
		[RegularExpression(@"^[0-9]+([.,][0-9]{1,2})?$", ErrorMessage = "Debe ser un número con hasta dos decimales.")]
		[Required(ErrorMessage = "Este campo es obligatorio.")]
		public decimal Precio { get; set; }
		
		[Required(ErrorMessage = "Este campo es obligatorio.")]
		public bool Estado { get; set; }

		[ForeignKey("PropietarioId")]
		public Propietario? Propietario{ get; set; }

        public string? Imagen { get; set; }

		[NotMapped]
		public IFormFile? archivoFoto { get; set; } 


		public override string ToString()
		{
			return $"{Direccion} - {Propietario?.Nombre} {Propietario?.Apellido}";
		}
		
    }
}